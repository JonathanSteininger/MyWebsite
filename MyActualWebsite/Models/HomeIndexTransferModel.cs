namespace MyActualWebsite.Models
{
    public class HomeIndexTransferModel
    {
        public HomeIndexTransferModel(List<StatBarCatagory> statBarCatagories,List<Project> projects, List<Experience> experiences)
        {
            this.StatBarCatagories = statBarCatagories;
            _statBars = new Dictionary<string, StatBar[]>();
            Projects = projects;
            Experiences = experiences;
        }
        public List<StatBarCatagory> StatBarCatagories { get; set; }

        private Dictionary<string, StatBar[]> _statBars;


        public List<Project> Projects { get; set; }
        public List<Experience> Experiences { get; set; }

        public void AddBarSet(StatBar[] bars)
        {
            if(bars.Length == 0) return;
            if (bars[0].StatBarCatagory == null) return;
            AddBarSet(bars[0].StatBarCatagory.Name, bars);
        }
        public bool BarSetsContainsKey(string Key) => _statBars.ContainsKey(Key);
        public void AddBarSet(string key, StatBar[] bars)
        {
            _statBars.Add(key, bars);
        }
        public StatBar[] GetBarSet(string Key)
        {
            if (!_statBars.ContainsKey(Key))
                throw new Exception("Key does not exsist in StatBarsDisctionary");
            return _statBars[Key];
        }
    }
}
