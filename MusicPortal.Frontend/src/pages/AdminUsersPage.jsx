import { useEffect, useState } from "react";
import { Link } from "react-router-dom";
import { api } from "../api/client";

export default function AdminUsersPage() {
  const [result, setResult] = useState(null);
  const [page, setPage] = useState(1);

  const load = () => api.getUsers(page).then(setResult).catch(() => {});

  useEffect(() => { load(); }, [page]);

  const handleDelete = async (id) => {
    if (!window.confirm("Удалить пользователя? Его исполнительский профиль и песни будут сохранены.")) return;
    await api.deleteUser(id);
    load();
  };

  if (!result) return <p>Загрузка...</p>;

  return (
    <div>
      <h2 className="mb-4">Пользователи</h2>
      <table className="table table-striped bg-white align-middle">
        <thead>
          <tr>
            <th>Имя</th><th>Email</th><th>Роли</th><th>Одобрен</th><th className="text-end">Действия</th>
          </tr>
        </thead>
        <tbody>
          {result.items.map((u) => (
            <tr key={u.id}>
              <td>{u.username}</td>
              <td>{u.email}</td>
              <td>{u.roles.map((r) => <span key={r.id} className="badge bg-secondary genre-badge me-1">{r.name}</span>)}</td>
              <td>
                {u.isApproved
                  ? <span className="badge bg-success">Да</span>
                  : <span className="badge bg-warning text-dark">Нет</span>}
              </td>
              <td className="text-end">
                <Link to={`/admin/users/${u.id}`} className="btn btn-sm btn-outline-secondary me-1">Изменить</Link>
                <button className="btn btn-sm btn-outline-danger" onClick={() => handleDelete(u.id)}>Удалить</button>
              </td>
            </tr>
          ))}
        </tbody>
      </table>

      {result.totalPages > 1 && (
        <nav className="mt-3">
          <ul className="pagination">
            {Array.from({ length: result.totalPages }, (_, i) => i + 1).map((p) => (
              <li key={p} className={`page-item ${p === result.page ? "active" : ""}`}>
                <button className="page-link" onClick={() => setPage(p)}>{p}</button>
              </li>
            ))}
          </ul>
        </nav>
      )}
    </div>
  );
}
