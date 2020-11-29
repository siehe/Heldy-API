using Heldy.DataAccess.Interfaces;
using Heldy.Models;
using Heldy.Models.Requests;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;

namespace Heldy.DataAccess
{
    public class CommentsRepository : ICommentsRespository
    {
        private DBConfig _dbConfig;

        public CommentsRepository()
        {
            _dbConfig = DbHelper.GetConfig();
        }

        public async Task CreateCommentAsync(CreateCommentRequest createCommentRequest)
        {
            await using var connection = new SqlConnection(_dbConfig.ConnectionString);
            await using var command = new SqlCommand("CreateComment", connection) { CommandType = System.Data.CommandType.StoredProcedure };

            await connection.OpenAsync();

            command.Parameters.AddWithValue("authorId", createCommentRequest.AuthorId);
            command.Parameters.AddWithValue("replyTo", createCommentRequest.ReplyTo);
            command.Parameters.AddWithValue("text", createCommentRequest.Text);
            command.Parameters.AddWithValue("createdAt", DateTime.UtcNow);

            await command.ExecuteNonQueryAsync();
        }

        public async Task<IEnumerable<Comment>> GetCommentsByPostId(int postId)
        {
            var result = new List<Comment>();

            await using var connection = new SqlConnection(_dbConfig.ConnectionString);
            await using var command = new SqlCommand("GetPostComments", connection) { CommandType = System.Data.CommandType.StoredProcedure };

            await connection.OpenAsync();

            command.Parameters.AddWithValue("postId", postId);

            await using var reader = await command.ExecuteReaderAsync();

            while (reader.Read())
            {
                result.Add(new Comment
                {
                    Id = reader.GetInt32(reader.GetOrdinal("Id")),
                    AuthorId = reader.GetInt32(reader.GetOrdinal("AuthorId")),
                    PostDate = reader.GetDateTime(reader.GetOrdinal("PostDate")),
                    ReplyTo = reader.GetInt32(reader.GetOrdinal("ReplyTo")),
                    Text = reader.GetString(reader.GetOrdinal("Text"))
                });
            }

            return result;
        }
    }
}
