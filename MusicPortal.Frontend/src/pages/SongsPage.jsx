import { useEffect, useState } from "react";
import { Link } from "react-router-dom";
import { api } from "../api/client";
import { useAuth } from "../context/AuthContext";

export default function SongsPage() {
    const { user, isInRole } = useAuth();
    const [result, setResult] = useState(null);
    const [genres, setGenres] = useState([]);
    const [artists, setArtists] = useState([]);
    const [genreId, setGenreId] = useState("");
    const [artistId, setArtistId] = useState("");
    const [sortBy, setSortBy] = useState("date");
    const [desc, setDesc] = useState(true);
    const [page, setPage] = useState(1);
    const [error, setError] = useState("");

    useEffect(() => {
        api.getGenres().then(setGenres).catch(() => { });
        api.getArtistsBrief().then(setArtists).catch(() => { });
    }, []);

    useEffect(() => {
        setError("");
        api
            .getSongs({ genreId, artistId, sortBy, desc, page })
            .then(setResult)
            .catch((e) => setError(e.message));
    }, [genreId, artistId, sortBy, desc, page]);

    const handleDelete = async (id) => {
        if (!window.confirm("Удалить песню?")) return;
        await api.deleteSong(id);
        setResult((r) => ({ ...r, items: r.items.filter((s) => s.id !== id) }));
    };

    const resetToFirstPage = (setter) => (value) => {
        setter(value);
        setPage(1);
    };

    return (
        <div>
            <div className="d-flex justify-content-between align-items-center mb-4">
                <h2>🎵 Каталог песен</h2>
                {user && (
                    <Link to="/songs/upload" className="btn btn-primary">
                        Загрузить песню
                    </Link>
                )}
            </div>

            {error && <div className="alert alert-danger">{error}</div>}

            <div className="card mb-4">
                <div className="card-body row g-3">
                    <div className="col-md-3">
                        <label className="form-label fw-bold">Жанр:</label>
                        <select
                            className="form-select"
                            value={genreId}
                            onChange={(e) => resetToFirstPage(setGenreId)(e.target.value)}
                        >
                            <option value="">Все жанры</option>
                            {genres.map((g) => (
                                <option key={g.id} value={g.id}>{g.name}</option>
                            ))}
                        </select>
                    </div>

                    <div className="col-md-3">
                        <label className="form-label fw-bold">Исполнитель:</label>
                        <select
                            className="form-select"
                            value={artistId}
                            onChange={(e) => resetToFirstPage(setArtistId)(e.target.value)}
                        >
                            <option value="">Все исполнители</option>
                            {artists.map((a) => (
                                <option key={a.id} value={a.id}>{a.name}</option>
                            ))}
                        </select>
                    </div>

                    <div className="col-md-3">
                        <label className="form-label fw-bold">Сортировка:</label>
                        <select
                            className="form-select"
                            value={sortBy}
                            onChange={(e) => resetToFirstPage(setSortBy)(e.target.value)}
                        >
                            <option value="title">По названию</option>
                            <option value="rating">По рейтингу</option>
                            <option value="plays">По прослушиваниям</option>
                            <option value="date">По дате</option>
                        </select>
                    </div>

                    <div className="col-md-3 d-flex align-items-end">
                        <div className="form-check">
                            <input
                                className="form-check-input"
                                type="checkbox"
                                checked={desc}
                                onChange={(e) => resetToFirstPage(setDesc)(e.target.checked)}
                                id="descCheck"
                            />
                            <label className="form-check-label" htmlFor="descCheck">По убыванию</label>
                        </div>
                    </div>
                </div>
            </div>

            {result?.items?.length ? (
                <>
                    <div className="row row-cols-1 row-cols-md-2 row-cols-lg-3 g-4">
                        {result.items.map((song) => (
                            <div className="col" key={song.id}>
                                <div className="card h-100 shadow-sm">
                                    <div className="card-body">
                                        <h5 className="card-title text-truncate">{song.title}</h5>
                                        <h6 className="card-subtitle mb-2 text-muted">{song.artistName}</h6>
                                        <p className="mb-1"><small className="text-muted">Рейтинг:</small> ⭐ {song.rating.toFixed(1)}</p>
                                        <p className="mb-3"><small className="text-muted">Прослушиваний:</small> 🎧 {song.playCount}</p>
                                        <audio controls className="w-100 mb-3" src={api.playUrl(song.id)} />
                                        <div className="d-flex justify-content-between align-items-center">
                                            <Link to={`/songs/${song.id}`} className="btn btn-sm btn-outline-primary">Подробнее</Link>
                                            {isInRole("Admin") && (
                                                <div className="btn-group">
                                                    <Link to={`/songs/${song.id}/edit`} className="btn btn-sm btn-outline-secondary">Редактировать</Link>
                                                    <button className="btn btn-sm btn-outline-danger" onClick={() => handleDelete(song.id)}>Удалить</button>
                                                </div>
                                            )}
                                        </div>
                                    </div>
                                </div>
                            </div>
                        ))}
                    </div>

                    {result.totalPages > 1 && (
                        <nav className="mt-4">
                            <ul className="pagination justify-content-center">
                                <li className={`page-item ${result.hasPrevious ? "" : "disabled"}`}>
                                    <button className="page-link" onClick={() => setPage((p) => p - 1)}>Назад</button>
                                </li>
                                {Array.from({ length: result.totalPages }, (_, i) => i + 1).map((p) => (
                                    <li key={p} className={`page-item ${p === result.page ? "active" : ""}`}>
                                        <button className="page-link" onClick={() => setPage(p)}>{p}</button>
                                    </li>
                                ))}
                                <li className={`page-item ${result.hasNext ? "" : "disabled"}`}>
                                    <button className="page-link" onClick={() => setPage((p) => p + 1)}>Вперёд</button>
                                </li>
                            </ul>
                        </nav>
                    )}
                </>
            ) : (
                <div className="text-center py-5">
                    <h4 className="text-muted">Песни не найдены</h4>
                </div>
            )}
        </div>
    );
}