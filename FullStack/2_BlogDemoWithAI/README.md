# FullStack Blog Demo

Ez a demo egy egyszerű, OpenAI-val és DeepL-lel integrált full-stack alkalmazást mutat be.

# Konfiguráció

- Hozz létre egy új `appsettings.json` fájlt a `Blog.Server` mappában található `appsettings.example.json` fájl alapján:
	- Hozz létre egy új OpenAI API tokent a [https://platform.openai.com/settings/profile?tab=api-keys](https://platform.openai.com/settings/profile?tab=api-keys) címen.
 		- Megjegyzés: Az aktuális szabályozás alapján ez akár költségeket is vonhat maga után, mindenképpen tájékozódj az OpenAI weboldalán az [aktuális felhasználási feltételekről](https://openai.com/api/pricing/)!	
	- Töltse ki az `API Key` mezőt a `Services/OpenAi` részben.
	- Hozz létre egy új DeepL API tokent a [https://www.deepl.com/en/your-account/keys](https://www.deepl.com/en/your-account/keys) címen.
	- Töltse ki az `API Key` mezőt a `Services/DeepL` részben.
- Futtasd a `dotnet publish -p:PublishProfile=FolderProfile` parancsot az alkalmazás közzétételéhez.
- Indítsd el az alkalmazást a `publish/Blog.Server.exe` állomány segítségével.
- Látogass el a `http://localhost:5000` címre a böngésző segítségével.
