namespace SimpleBot.Repo
{
    public interface IUserRepo
    {
        void SalvarHistorico(Message message);

        UserProfile GetProfile(string id);

        void SetProfile(string id, UserProfile profile);
    }
}
