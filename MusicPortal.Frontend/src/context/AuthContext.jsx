import { createContext, useContext, useEffect, useState } from "react";
import { api } from "../api/client";

const AuthContext = createContext(null);

export function AuthProvider({ children }) {
  const [user, setUser] = useState(null);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    api
      .me()
      .then(setUser)
      .catch(() => setUser(null))
      .finally(() => setLoading(false));
  }, []);

  const login = async (email, password, rememberMe) => {
    const userDto = await api.login(email, password, rememberMe);
    setUser({
      id: userDto.id,
      username: userDto.username,
      email: userDto.email,
      roles: (userDto.roles || []).map((r) => r.name),
    });
    return userDto;
  };

  const logout = async () => {
    await api.logout();
    setUser(null);
  };

  const isInRole = (role) => !!user?.roles?.includes(role);

  return (
    <AuthContext.Provider value={{ user, loading, login, logout, isInRole }}>
      {children}
    </AuthContext.Provider>
  );
}

export function useAuth() {
  const ctx = useContext(AuthContext);
  if (!ctx) throw new Error("useAuth must be used within AuthProvider");
  return ctx;
}
