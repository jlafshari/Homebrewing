namespace BeerRecipeCore
{
    public class BeerXmlObject
    {
        public BeerXmlObject(int id, string name, int version, string notes)
        {
            m_id = id;
            m_name = name;
            m_version = version;
            m_notes = notes;
        }

        public int Id
        {
            get { return m_id; }
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

        private int m_id;
        private string m_name = "";
        private int m_version;
        private string m_notes = "";
    }
}
