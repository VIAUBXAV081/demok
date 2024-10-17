class DissolutionCurve:

    def __init__(self, name):
        self.name = name
        self.time = []
        self.values = []

    def __str__(self):
        return f'<DissolutionCurve: {self.name}>'

    def __repr__(self):
        return self.__str__()


class RamanMap:

    def __init__(self, name):
        self.name = name
        self.components = {}

    def __str__(self):
        return f'<RamanMap: {self.name}>'

    def __repr__(self):
        return self.__str__()
