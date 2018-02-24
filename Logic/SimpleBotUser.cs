using SimpleBot.Repo;
using System.Collections.Generic;
using System.Configuration;

namespace SimpleBot
{
    public class SimpleBotUser
    {
        static Dictionary<string, UserProfile> _perfil = new Dictionary<string, UserProfile>();
        //static IUserRepo _userRepo = new UserMongoRepo();
        static IUserRepo _userRepo = new UserSqlRepo(ConfigurationManager.AppSettings["sql"]);
        public static string Reply(Message message)
        {
            string userId = message.Id;

            var perfil = _userRepo.GetProfile(userId);

            perfil.Visitas += 1;

            _userRepo.SetProfile(userId, perfil);

            _userRepo.SalvarHistorico(message);

            return $"{message.User} conversou '{perfil.Visitas}'";
        }
    }
}