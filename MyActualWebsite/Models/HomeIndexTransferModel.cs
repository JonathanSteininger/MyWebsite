namespace MyActualWebsite.Models
{
    public class HomeIndexTransferModel
    {
        public HomeIndexTransferModel(List<StatBar> statBars, List<Project> projects)
        {
            StatBars = statBars;
            Projects = projects;
        }

        public List<StatBar> StatBars { get; set; }
        public List<Project> Projects { get; set; }
    }
}
