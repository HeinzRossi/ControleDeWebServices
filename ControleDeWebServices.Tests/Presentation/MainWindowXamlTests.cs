using FluentAssertions;
using System.Text.RegularExpressions;

namespace ControleDeWebServices.Tests.Presentation;

public class MainWindowXamlTests
{
    [Fact]
    public void ConfirmDialogOverlay_NaoPodeTerVisibilityLocal()
    {
        var xaml = File.ReadAllText(GetMainWindowPath());
        var overlayMatch = Regex.Match(
            xaml,
            "<Grid\\s+Grid\\.ColumnSpan=\"2\"(?<overlay>.*?)<Grid\\.Style>",
            RegexOptions.Singleline);

        overlayMatch.Success.Should().BeTrue();
        overlayMatch.Groups["overlay"].Value.Should().NotContain("Visibility=\"Collapsed\"");
    }

    private static string GetMainWindowPath()
    {
        var current = new DirectoryInfo(AppContext.BaseDirectory);
        while (current != null && !File.Exists(Path.Combine(current.FullName, "ControleDeWebServices.sln")))
        {
            current = current.Parent;
        }

        current.Should().NotBeNull();
        return Path.Combine(current!.FullName, "ControleDeWebServices", "MainWindow.xaml");
    }
}
