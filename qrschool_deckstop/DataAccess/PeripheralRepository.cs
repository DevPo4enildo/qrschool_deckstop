using Dapper;
using qrschool_deckstop.Models;
using System.Collections.Generic;
using System.Linq;

namespace qrschool_deckstop.DataAccess
{
    public class PeripheralRepository : IRepository<Peripheral>
    {
        public IEnumerable<Peripheral> GetAll()
        {
            using (var connection = DatabaseContext.CreateConnection())
            {
            var sql = @"
                SELECT p.*, r.name as RoomName, e.full_name as ResponsibleEmployee
                FROM peripherals p
                LEFT JOIN rooms r ON p.room_id = r.id
                LEFT JOIN employees e ON p.responsible_employee_id = e.id
                ORDER BY p.id";
            return connection.Query<Peripheral>(sql);
            }
        }

        public Peripheral GetById(int id)
        {
            using (var connection = DatabaseContext.CreateConnection())
            {
            var sql = "SELECT * FROM peripherals WHERE id = @Id";
            return connection.QueryFirstOrDefault<Peripheral>(sql, new { Id = id });
            }
        }

        public int Add(Peripheral entity)
        {
            using (var connection = DatabaseContext.CreateConnection())
            {
            var sql = @"
                INSERT INTO peripherals (
                    code, inventory_no, room_id, computer_id, type, brand, model,
                    serial_number, status, comment, cost, purchase_date, warranty_until,
                    responsible_employee_id, notes, created_at, updated_at, created_by,
                    updated_by, sync_status
                ) VALUES (
                    @Code, @InventoryNo, @RoomId, @ComputerId, @Type, @Brand, @Model,
                    @SerialNumber, @Status, @Comment, @Cost, @PurchaseDate, @WarrantyUntil,
                    @ResponsibleEmployeeId, @Notes, @CreatedAt, @UpdatedAt, @CreatedBy,
                    @UpdatedBy, @SyncStatus
                ) RETURNING id";
            return connection.QuerySingle<int>(sql, entity);
            }
        }

        public bool Update(Peripheral entity)
        {
            using (var connection = DatabaseContext.CreateConnection())
            {
            var sql = @"
                UPDATE peripherals SET
                    code = @Code,
                    inventory_no = @InventoryNo,
                    room_id = @RoomId,
                    computer_id = @ComputerId,
                    type = @Type,
                    brand = @Brand,
                    model = @Model,
                    serial_number = @SerialNumber,
                    status = @Status,
                    comment = @Comment,
                    cost = @Cost,
                    purchase_date = @PurchaseDate,
                    warranty_until = @WarrantyUntil,
                    responsible_employee_id = @ResponsibleEmployeeId,
                    notes = @Notes,
                    updated_at = @UpdatedAt,
                    updated_by = @UpdatedBy,
                    sync_status = @SyncStatus
                WHERE id = @Id";
            return connection.Execute(sql, entity) > 0;
            }
        }

        public bool Delete(int id)
        {
            using (var connection = DatabaseContext.CreateConnection())
            {
            var sql = "DELETE FROM peripherals WHERE id = @Id";
            return connection.Execute(sql, new { Id = id }) > 0;
            }
        }
    }
}
