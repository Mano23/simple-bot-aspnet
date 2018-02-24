using System.Collections.Generic;

namespace SimpleBot.Repo
{
    public class UserMemRepo : IUserRepo
    {
        List<string> _log = new List<string>();
        static Dictionary<string, UserProfile> _perfil = new Dictionary<string, UserProfile>();

        public UserProfile GetProfile(string id)
        {
            if (_perfil.ContainsKey(id))
                return _perfil[id];
            return new UserProfile()
            {
                Id = id,
                Visitas = 0
            };
        }

        public void SalvarHistorico(Message message)
        {
            _log.Add(message.Text);
        }

        public void SetProfile(string id, UserProfile profile)
        {
            _perfil[id] = profile;
        }
    }
}