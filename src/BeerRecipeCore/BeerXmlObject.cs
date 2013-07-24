namespace BeerRecipeCore
{
    public class BeerXmlObject
    {
        public BeerXmlObject(string name, int version, string notes)
        {
            m_name = name;
            m_version = version;
            m_notes = notes;
        }

        public string Name
        {
            get { return m_name; }
        }

        public int Version
        {
            get { return m_version; }
        }

        public string Notes
        {
            get { return m_notes; }
        }

        private string m_name = "";
        private int m_version;
        private string m_notes = "";
    }
}
