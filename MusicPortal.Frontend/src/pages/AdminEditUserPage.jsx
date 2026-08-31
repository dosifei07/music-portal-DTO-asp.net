import { useEffect, useState } from "react";
import { useNavigate, useParams } from "react-router-dom";
import { api } from "../api/client";

export default function AdminEditUserPage() {
  const { id } = useParams();
  const navigate = useNavigate();
  const [user, setUser] = useState(null);
  const [allRoles, setAllRoles] = useState([]);
  const [roleIds, setRoleIds] = useState([]);
  const [isApproved, setIsApproved] = useState(false);
  const [error, setError] = useState("");

  useEffect(() => {
    api.getUser(id).then((u) => {
      setUser(u);
      setRoleIds(u.roles.map((r) => r.id));
      setIsApproved(u.isApproved);
    }).catch((e) => setError(e.message));
    api.getRoles().then(setAllRoles).catch(() => {});
  }, [id]);

  const toggleRole = (rid) => {
    setRoleIds((prev) => (prev.includes(rid) ? prev.filter((r) => r !== rid) : [...prev, rid]));
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    try {
      await api.updateUser(id, roleIds, isApproved);
      navigate("/admin/users");
    } catch (err) {
      setError(err.message);
    }
  };

  if (!user) return <p>Загрузка...</p>;

  return (
    <div className="row justify-content-center">
      <div className="col-md-6">
        <h2 className="mb-4">{user.username}</h2>
        {error && <div className="alert alert-danger">{error}</div>}
        <form onSubmit={handleSubmit}>
          <p className="text-muted">Email: {user.email}</p>

          <div className="mb-3">
            <label className="form-label">Роли</label>
            {allRoles.map((role) => (
              <div className="form-check" key={role.id}>
                <input
                  type="checkbox"
                  className="form-check-input"
                  id={`role-${role.id}`}
                  checked={roleIds.includes(role.id)}
                  onChange={() => toggleRole(role.id)}
                />
                <label className="form-check-label" htmlFor={`role-${role.id}`}>{role.name}</label>
              </div>
            ))}
          </div>

          <div className="form-check mb-3">
            <input type="checkbox" className="form-check-input" id="isApproved" checked={isApproved} onChange={(e) => setIsApproved(e.target.checked)} />
            <label className="form-check-label" htmlFor="isApproved">Одобрен</label>
          </div>

          <button type="submit" className="btn btn-primary">Сохранить</button>
          <button type="button" className="btn btn-outline-secondary ms-2" onClick={() => navigate("/admin/users")}>Отмена</button>
        </form>
      </div>
    </div>
  );
}
