using YamlDotNet.Serialization.NamingConventions;
using YamlDotNet.Serialization;
using System.Text;
using System.Web;

Console.WriteLine("Starting to create the https://principles-of-ethical-hacking.org Website.");

var websiteOutput = "./output/";

if (!Directory.Exists(websiteOutput))
{
    Directory.CreateDirectory(websiteOutput);
}

CopyDirectory(@"website/assets/", Path.Combine(websiteOutput, "assets"));

var hackerTemplatePath = @"website/template/hacker.html";
var organisationTemplatePath = @"website/template/organisation.html";
var yamlPath = @"website/data/i18n/";
var yamlFiles = Directory.GetFiles(yamlPath, "*.yaml");

var languages = GetAllLanguages(yamlFiles);

foreach (var yamlFile in yamlFiles)
{
    var file = new FileInfo(yamlFile);
    var language = file.Name.ToLower().Replace(file.Extension.ToLower(), string.Empty);
    var yamlContent = File.ReadAllText(yamlFile);

    var deserializer = new DeserializerBuilder()
                    .WithNamingConvention(CamelCaseNamingConvention.Instance)
                    .Build();

    var i18nData = deserializer.Deserialize<Dictionary<string, Dictionary<string, Dictionary<string, string>>>>(yamlContent);

    // Load Website

    string hackerWebsite = File.ReadAllText(hackerTemplatePath);
    string organisationWebsite = File.ReadAllText(organisationTemplatePath);

    // Search and Replace

    var langBuilder = new StringBuilder();
    var langFirstRound = true;
    foreach (var lang in languages.OrderBy(p => p.ToString()))
    {
        if (!langFirstRound)
        {
            langBuilder.Append(" | ");
        }
        else
        {
            langFirstRound = false;
        }

        var currentLanguage = $"<a href=\"/{lang.ToLower()}/[[SECTION]]\">{lang.ToUpper()}</a>";
        if (lang.ToLower().Equals(language.ToLower()))
        {
            currentLanguage = $"<span class=\"badge text-bg-secondary\">{lang.ToUpper()}</span>";
        }

        langBuilder.Append(currentLanguage);
    }

    foreach (var section in i18nData)
    {
        Console.WriteLine($"Section: {section.Key}");
        var languageFolder = Path.Combine(websiteOutput, language);

        if (!Directory.Exists(languageFolder)) {
            Directory.CreateDirectory(languageFolder);
        }

        if (!string.IsNullOrEmpty(section.Key) && section.Key.ToLower().Equals("hacker"))
        {
            foreach (var subsection in section.Value)
            {
                if (subsection.Value.TryGetValue("title", out var title) && subsection.Value.TryGetValue("description", out var description))
                {
                    hackerWebsite = hackerWebsite.Replace($"[[{section.Key}.title.{subsection.Key}]]", HttpUtility.HtmlEncode(title));
                    hackerWebsite = hackerWebsite.Replace($"[[{section.Key}.description.{subsection.Key}]]", HttpUtility.HtmlEncode(description));
                }
            }

            hackerWebsite = hackerWebsite.Replace("[[LANG]]", language.ToLower());
            hackerWebsite = hackerWebsite.Replace("[[LANGUAGES]]", langBuilder.ToString());
            hackerWebsite = hackerWebsite.Replace("[[SECTION]]", HttpUtility.HtmlEncode(section.Key));

            // Save on new location
            string targetFilePath = Path.Combine(websiteOutput, language, "hacker.html");

            File.WriteAllText(targetFilePath, hackerWebsite);

            if (language.ToLower().Equals("en"))
            {
                string targetFilePathIndex = Path.Combine(websiteOutput, "index.html");
                File.WriteAllText(targetFilePathIndex, hackerWebsite);
            }
        }

        if (!string.IsNullOrEmpty(section.Key) && section.Key.ToLower().Equals("organisation"))
        {
            foreach (var subsection in section.Value)
            {
                if (subsection.Value.TryGetValue("title", out var title) && subsection.Value.TryGetValue("description", out var description))
                {
                    organisationWebsite = organisationWebsite.Replace($"[[{section.Key}.title.{subsection.Key}]]", HttpUtility.HtmlEncode(title));
                    organisationWebsite = organisationWebsite.Replace($"[[{section.Key}.description.{subsection.Key}]]", HttpUtility.HtmlEncode(description));
                }
            }

            organisationWebsite = organisationWebsite.Replace("[[LANG]]", language.ToLower());
            organisationWebsite = organisationWebsite.Replace("[[LANGUAGES]]", langBuilder.ToString());
            organisationWebsite = organisationWebsite.Replace("[[SECTION]]", HttpUtility.HtmlEncode(section.Key));

            // Save on new location
            string targetFilePath = Path.Combine(websiteOutput, language, "organisation.html");
            File.WriteAllText(targetFilePath, organisationWebsite);
        }
    }
}


static void CopyDirectory(string sourceDir, string destinationDir)
{
    if (!Directory.Exists(destinationDir))
    {
        Directory.CreateDirectory(destinationDir);
    }

    foreach (var file in Directory.GetFiles(sourceDir))
    {
        string targetFilePath = Path.Combine(destinationDir, Path.GetFileName(file));
        File.Copy(file, targetFilePath, true);
    }

    foreach (var directory in Directory.GetDirectories(sourceDir))
    {
        string targetDirectoryPath = Path.Combine(destinationDir, Path.GetFileName(directory));
        CopyDirectory(directory, targetDirectoryPath);
    }
}

static List<string> GetAllLanguages(string[] yamlFiles)
{
    var listLang = new List<string>();
    foreach (var yamlFile in yamlFiles)
    {
        var file = new FileInfo(yamlFile);
        listLang.Add(file.Name.ToLower().Replace(file.Extension.ToLower(), string.Empty));
    }

    return listLang;
}