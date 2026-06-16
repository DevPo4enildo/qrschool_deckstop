using Dapper;
using qrschool_deckstop.Models;
using System.Collections.Generic;
using System.Linq;

namespace qrschool_deckstop.DataAccess
{
    public class ComputerRepository : IRepository<Computer>
    {
        public IEnumerable<Computer> GetAll()
        {
            using (var connection = DatabaseContext.CreateConnection())
            {
            var sql = @"
                SELECT c.*, r.name as RoomName, e.full_name as ResponsibleEmployee
                FROM public.""computers"" c
                LEFT JOIN rooms r ON c.room_id = r.id
                LEFT JOIN employees e ON c.responsible_employee_id = e.id
                ORDER BY c.id";
            return connection.Query<Computer>(sql);
            }
        }

        public Computer GetById(int id)
        {
            using (var connection = DatabaseContext.CreateConnection())
            {
            var sql = "SELECT * FROM public.\"computers\" WHERE id = @Id";
            return connection.QueryFirstOrDefault<Computer>(sql, new { Id = id });
            }
        }

        public int Add(Computer entity)
        {
            using (var connection = DatabaseContext.CreateConnection())
            {
            var sql = @"
                INSERT INTO public.""computers"" (
                    code, inventory_no, room_id, computer_id, brand, model, diagonal_inch,
                    serial_number, status, comment, cost, purchase_date, warranty_until,
                    responsible_employee_id, notes, created_at, updated_at, created_by,
                    updated_by, sync_status
                ) VALUES (
                    @Code, @InventoryNo, @RoomId, @ComputerId, @Brand, @Model, @DiagonalInch,
                    @SerialNumber, @Status, @Comment, @Cost, @PurchaseDate, @WarrantyUntil,
                    @ResponsibleEmployeeId, @Notes, @CreatedAt, @UpdatedAt, @CreatedBy,
                    @UpdatedBy, @SyncStatus
                ) RETURNING id";
            return connection.QuerySingle<int>(sql, entity);
            }
        }

        public bool Update(Computer entity)
        {
            using (var connection = DatabaseContext.CreateConnection())
            {
            var sql = @"
                UPDATE public.""computers"" SET
                    code = @Code,
                    inventory_no = @InventoryNo,
                    room_id = @RoomId,
                    computer_id = @ComputerId,
                    brand = @Brand,
                    model = @Model,
                    diagonal_inch = @DiagonalInch,
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
            var sql = "DELETE FROM public.\"computers\" WHERE id = @Id";
            return connection.Execute(sql, new { Id = id }) > 0;
            }
        }
    }
}
