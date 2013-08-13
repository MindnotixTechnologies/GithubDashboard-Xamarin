# A Shinobi Controls GitHub Dashboard


A dashboard built around the Github API using ShinobiControls and Xamarin.iOS bindings.

## Getting Started

TODO: add instructions to obtain required frameworks here. This include licence key requirements.

### GitHub Personal Access Token

Without authentication GitHub limits your IP address to 60 requests per hour. This is a problem since changing the repo in the dashboard app requires at least 5 requests. Authenticating against github enables you to perform up to 5000 requests per hour. Therefore we check here whether or not credentials have been provided and if they have then we'll use them.

To create credentials for the app (based on https://help.github.com/articles/creating-an-access-token-for-command-line-use):

  1. Visit https://github.com/settings/applications
  2. Click create new token
  3. Give it an appropriate name, and copy the generated token.
  4. Using `GithubAuthenticationToken.sample.json` as a template create `GithubAuthenticationToken.json` - with your username and token pasted in the appropriate places.
  5. Right click on the JSON file in Xamarin Studio, and ensure that under build action "BundleResource" is selected.
  6. Ensure that you don't commit the JSON file into source control. 
  
Please note that this solution is not suitable for production applications - for these you should implement a user-based OAuth2 solution, as per the instructions on GitHub.