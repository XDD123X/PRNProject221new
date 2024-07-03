namespace ProjectPRN221.SampleUserRedis
{
    public class SampleRepository
    {
        public async Task<string> GetUsersAsync()
        {
            string output = "accesToken";

            await Task.Delay(3000); // simulating data access time

            return output;
        }
    }
}
