import glob
import json
import zipfile
from io import StringIO
from types import SimpleNamespace
from .models import *
import numpy as np


def load_dataset(raman_path, dissolution_path):
    items = load_dataset_objects(raman_path, dissolution_path).items()

    return (
        np.array([data.get('Map').components.get('HPMC') for (key, data) in items]),
        np.array([data.get('Curve').values for (key, data) in items])
    )


def load_dataset_objects(raman_path, dissolution_path):
    maps = {load_raman_map_from_file(path) for path in glob.glob(raman_path)}
    curves = {load_dissolution_curve_from_file(path) for path in glob.glob(dissolution_path)}
    items = {
        map.name: {
            'Map': map,
            'Curve': filter(lambda item: map.name == item.name, curves).__next__()
        }
        for map in maps
    }
    return dict(sorted(items.items()))


def load_raman_map_from_file(path):
    input_file = zipfile.ZipFile(path)
    meta = json.loads(input_file.read('meta.json'), object_hook=lambda d: SimpleNamespace(**d))
    raman = RamanMap(meta.Name)

    for c in meta.Components:
        comp_csv = StringIO(input_file.read(c.File.Path).decode())
        matrix = np.genfromtxt(comp_csv, delimiter=',')
        raman.components.__setitem__(c.Name, matrix)

    return raman


def load_dissolution_curve_from_file(path):
    with open(path, 'r') as f:
        obj = json.load(f, object_hook=lambda d: SimpleNamespace(**d))
        dc = DissolutionCurve(obj.Name)
        dc.time = np.array(obj.Time)
        dc.values = np.array(obj.Values)
    return dc

