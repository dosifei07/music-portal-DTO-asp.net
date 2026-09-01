import { Navigate, useLocation } from "react-router-dom";
import { useAuth } from "../context/AuthContext";

export function RequireAuth({ children, role }) {
  const { user, loading, isInRole } = useAuth();
  const location = useLocation();

  if (loading) return <p>Загрузка...</p>;

  if (!user) {
    return <Navigate to="/login" state={{ from: location }} replace />;
  }

  if (role && !isInRole(role)) {
    return <Navigate to="/" replace />;
  }

  return children;
}