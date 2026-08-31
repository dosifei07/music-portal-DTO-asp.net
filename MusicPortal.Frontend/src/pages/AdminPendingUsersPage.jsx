import { useEffect, useState } from "react";
import { api } from "../api/client";

export default function AdminPendingUsersPage() {
  const [pending, setPending] = useState(null);

  const load = () => api.getPendingUsers().then(setPending).catch(() => {});
  useEffect(() => { load(); }, []);

  const approve = async (id) => { await api.approveUser(id); load(); };
  const reject = async (id) => {
    if (!window.confirm("Отклонить и удалить заявку?")) return;
    await api.rejectUser(id);
    load();
  };

  if (!pending) return <p>Загрузка...</p>;

  return (
    <div>
      <h2 className="mb-4">Заявки на регистрацию</h2>
      {pending.length === 0 && <p className="text-muted">Новых заявок нет.</p>}
      <table className="table table-striped bg-white align-middle">
        <thead>
          <tr><th>Имя</th><th>Email</th><th>Дата регистрации</th><th className="text-end">Действия</th></tr>
        </thead>
        <tbody>
          {pending.map((u) => (
            <tr key={u.id}>
              <td>{u.username}</td>
              <td>{u.email}</td>
              <td>{new Date(u.createdAt).toLocaleDateString("ru-RU")}</td>
              <td className="text-end">
                <button className="btn btn-sm btn-success me-1" onClick={() => approve(u.id)}>Одобрить</button>
                <button className="btn btn-sm btn-outline-danger" onClick={() => reject(u.id)}>Отклонить</button>
              </td>
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  );
}
