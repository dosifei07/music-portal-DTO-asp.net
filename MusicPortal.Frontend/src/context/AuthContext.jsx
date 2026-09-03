import { createContext, useContext, useEffect, useState } from "react";
import { api } from "../api/client";

const AuthContext = createContext(null);

const normalizeUser = (userDto) => {
    if (!userDto) return null;

    const roles = (userDto.roles || []).map((r) =>
        typeof r === "object" && r !== null ? r.name : r
    );

    return {
        id: userDto.id,
        username: userDto.username,
        email: userDto.email,
        roles,
    };
};
export function AuthProvider({ children }) {
    const [user, setUser] = useState(null);
    const [loading, setLoading] = useState(true);

    useEffect(() => {
        api
            .me()
            .then((data) =>
                setUser({
                    id: data.id,
                    username: data.username,
                    email: data.email,
                    roles: data.roles || [],
                })
            )
            .catch(() => setUser(null))
            .finally(() => setLoading(false));
    }, []);

    const login = async (email, password, rememberMe) => {
        const userDto = await api.login(email, password, rememberMe);

        const roles = (userDto.roles || []).map((r) => (typeof r === "object" ? r.name : r));

        setUser({
            id: userDto.id,
            username: userDto.username,
            email: userDto.email,
            roles,
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