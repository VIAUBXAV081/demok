# FullStack Blog Demo

This demo showcases a simple full-stack application integrated with OpenAI to generate post suggestions based on the provided title.

# Configuration

- Create a new OpenAI API token at [https://platform.openai.com/settings/profile?tab=api-keys](https://platform.openai.com/settings/profile?tab=api-keys).
- Create a new `appsettings.json` file based on the `appsettings.example.json` in the `Blog.Server` folder.
- Fill in the `API Key` under the `Services/OpenAi` section.
- Run the `dotnet publish -p:PublishProfile=FolderProfile` command to publish the application.
- Start the application using the `publish/Blog.Server.exe` executable.
- Browse to `http://localhost:5000`. 