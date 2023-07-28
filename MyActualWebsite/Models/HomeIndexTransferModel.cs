namespace MyActualWebsite.Models
{
    public class HomeIndexTransferModel
    {
        public HomeIndexTransferModel(List<StatBar> statBars, List<Project> projects, List<Experience> experiences)
        {
            StatBars = statBars;
            Projects = projects;
            Experiences = experiences;
        }

        public List<StatBar> StatBars { get; set; }
        public List<Project> Projects { get; set; }

        public List<Experience> Experiences { get; set; }
    }
}
