# A Shinobi Controls GitHub Dashboard


A dashboard built around the Github API using ShinobiControls and Xamarin.iOS bindings.

## Getting Started

### Shinobi licence keys

This project has been built using the trial versions of ShinobiCharts and ShinobiGrids - available from the Xamarin component store. Therefore, when you open the solution file in Xamarin Studio, the components should be magically installed for you.

Since we're using the trial versions you'll be issued with licence keys for each component when they are first downloaded from the component store. You can access these licence keys by logging in to the component store via the web interface, selecting your account and viewing 'My Components'. Form here you can select your trail components fro your list of components in order to find your trial keys.

In order to prevent the license key having to be repeatedly pasted into each class which uses a Shinobi control, this project uses a shared class to read the license key from a JSON file at startup and then push it into charts and grids as required. The process is very similar to the process for adding a github token (specified below):
 
  1. Using `AppSecrets.sample.json` as a template, create `AppSecrets.json`, with you license keys pasted into the appropriate places within the `shinobi_license_keys` section of the file.
  2. Right click on the JSON file in Xamarin Studio, and ensure that under build action "BundleResource" is selected.
  3. Ensure that you don't commit the new JSON file to your source control.

If you own ShinobiCharts and/or Grids then you won't need license keys, and can safely ignore these instructions.

### GitHub Personal Access Token

Without authentication GitHub limits your IP address to 60 requests per hour. This is a problem since changing the repo in the dashboard app requires at least 5 requests. Authenticating against github enables you to perform up to 5000 requests per hour. Therefore we check here whether or not credentials have been provided and if they have then we'll use them.

To create credentials for the app (based on https://help.github.com/articles/creating-an-access-token-for-command-line-use):

  1. Visit https://github.com/settings/applications
  2. Click create new token
  3. Give it an appropriate name, and copy the generated token.
  4. Using `AppSecrets.sample.json` as a template create `AppSecrets.json` - with your username and token pasted in the appropriate places - as part of the `github_authentication_token` dictionary. 
  5. Right click on the JSON file in Xamarin Studio, and ensure that under build action "BundleResource" is selected.
  6. Ensure that you don't commit the JSON file into source control. 
  
Please note that this solution is not suitable for production applications - for these you should implement a user-based OAuth2 solution, as per the instructions on GitHub.
