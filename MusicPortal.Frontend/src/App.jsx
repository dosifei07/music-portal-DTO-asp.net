import { BrowserRouter, Routes, Route, Link, useNavigate } from "react-router-dom";
import { AuthProvider, useAuth } from "./context/AuthContext";
import { RequireAuth } from "./components/RequireAuth";

import SongsPage from "./pages/SongsPage";
import SongDetailsPage from "./pages/SongDetailsPage";
import SongUploadPage from "./pages/SongUploadPage";
import LoginPage from "./pages/LoginPage";
import RegisterPage from "./pages/RegisterPage";
import AdminUsersPage from "./pages/AdminUsersPage";
import AdminEditUserPage from "./pages/AdminEditUserPage";
import AdminPendingUsersPage from "./pages/AdminPendingUsersPage";

function NavBar() {
    const { user, isInRole, logout } = useAuth();
    const navigate = useNavigate();

    return (
        <nav className="navbar navbar-expand-lg navbar-dark bg-dark mb-4">
            <div className="container">
                <Link className="navbar-brand" to="/">Музыкальный портал</Link>
                <div className="collapse navbar-collapse show">
                    <ul className="navbar-nav me-auto">
                        <li className="nav-item"><Link className="nav-link" to="/">Песни</Link></li>
                        {user && <li className="nav-item"><Link className="nav-link" to="/songs/upload">Загрузить песню</Link></li>}
                        {isInRole("Admin") && (
                            <>
                                <li className="nav-item"><Link className="nav-link" to="/admin/users">Пользователи</Link></li>
                                <li className="nav-item"><Link className="nav-link" to="/admin/pending">Заявки</Link></li>
                            </>
                        )}
                    </ul>
                    <ul className="navbar-nav">
                        {user ? (
                            <>
                                <li className="nav-item navbar-text text-light me-3">👤 {user.username}</li>
                                <li className="nav-item">
                                    <button className="btn btn-outline-light btn-sm" onClick={async () => { await logout(); navigate("/"); }}>
                                        Выйти
                                    </button>
                                </li>
                            </>
                        ) : (
                            <>
                                <li className="nav-item"><Link className="nav-link" to="/login">Войти</Link></li>
                                <li className="nav-item"><Link className="nav-link" to="/register">Регистрация</Link></li>
                            </>
                        )}
                    </ul>
                </div>
            </div>
        </nav>
    );
}

export default function App() {
    return (
        <AuthProvider>
            <BrowserRouter>
                <NavBar />
                <div className="container pb-5">
                    <Routes>
                        <Route path="/" element={<SongsPage />} />
                        <Route path="/songs/:id" element={<SongDetailsPage />} />
                        <Route path="/songs/upload" element={<RequireAuth><SongUploadPage /></RequireAuth>} />
                        <Route path="/login" element={<LoginPage />} />
                        <Route path="/register" element={<RegisterPage />} />
                        <Route path="/admin/users" element={<RequireAuth role="Admin"><AdminUsersPage /></RequireAuth>} />
                        <Route path="/admin/users/:id" element={<RequireAuth role="Admin"><AdminEditUserPage /></RequireAuth>} />
                        <Route path="/admin/pending" element={<RequireAuth role="Admin"><AdminPendingUsersPage /></RequireAuth>} />
                    </Routes>
                </div>
            </BrowserRouter>
        </AuthProvider>
    );
}