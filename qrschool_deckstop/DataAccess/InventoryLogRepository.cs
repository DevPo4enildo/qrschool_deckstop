using Dapper;
using qrschool_deckstop.Models;
using System;

namespace qrschool_deckstop.DataAccess
{
    public class InventoryLogRepository
    {
        public void AddLog(string objectType, int objectId, string operation, int? userId, string details)
        {
            var connection = DatabaseContext.CreateConnection();
            var sql = @"
                INSERT INTO inventory_log (
                    object_type, object_id, operation, user_id, details, created_at, sync_status
                ) VALUES (
                    @ObjectType, @ObjectId, @Operation, @UserId, @Details, @CreatedAt, @SyncStatus
                )";
            connection.Execute(sql, new
            {
                ObjectType = objectType,
                ObjectId = objectId,
                Operation = operation,
                UserId = userId,
                Details = details,
                CreatedAt = DateTime.Now,
                SyncStatus = "synced"
            });
        }
    }
}
