import { useEffect, useState } from "react";
import { useNavigate, useParams } from "react-router-dom";
import { api } from "../api/client";

export default function SongEditPage() {
    const { id } = useParams();
    const navigate = useNavigate();
    const [title, setTitle] = useState("");
    const [artistId, setArtistId] = useState("");
    const [genreIds, setGenreIds] = useState([]);
    const [artists, setArtists] = useState([]);
    const [genres, setGenres] = useState([]);
    const [error, setError] = useState("");

    useEffect(() => {
        api.getSong(id).then((song) => {
            setTitle(song.title);
            setArtistId(song.artistId);
            setGenreIds(song.genres.map((g) => g.id));
        }).catch((e) => setError(e.message));
        api.getArtistsBrief().then(setArtists).catch(() => { });
        api.getGenres().then(setGenres).catch(() => { });
    }, [id]);

    const toggleGenre = (gid) => {
        setGenreIds((prev) => (prev.includes(gid) ? prev.filter((g) => g !== gid) : [...prev, gid]));
    };

    const handleSubmit = async (e) => {
        e.preventDefault();
        setError("");
        try {
            await api.updateSong(id, { id: Number(id), title, artistId: Number(artistId), genreIds });
            navigate(`/songs/${id}`);
        } catch (err) {
            setError(err.message);
        }
    };

    return (
        <div className="row justify-content-center">
            <div className="col-md-6">
                <h2 className="mb-4">Редактирование песни</h2>
                {error && <div className="alert alert-danger">{error}</div>}
                <form onSubmit={handleSubmit}>
                    <div className="mb-3">
                        <label className="form-label">Название</label>
                        <input className="form-control" value={title} onChange={(e) => setTitle(e.target.value)} required maxLength={150} />
                    </div>

                    <div className="mb-3">
                        <label className="form-label">Исполнитель</label>
                        <select className="form-select" value={artistId} onChange={(e) => setArtistId(e.target.value)} required>
                            {artists.map((a) => (
                                <option key={a.id} value={a.id}>{a.name}</option>
                            ))}
                        </select>
                    </div>

                    <div className="mb-3">
                        <label className="form-label">Жанры</label>
                        {genres.map((g) => (
                            <div className="form-check" key={g.id}>
                                <input
                                    type="checkbox"
                                    className="form-check-input"
                                    id={`genre-${g.id}`}
                                    checked={genreIds.includes(g.id)}
                                    onChange={() => toggleGenre(g.id)}
                                />
                                <label className="form-check-label" htmlFor={`genre-${g.id}`}>{g.name}</label>
                            </div>
                        ))}
                    </div>

                    <button type="submit" className="btn btn-primary">Сохранить</button>
                    <button type="button" className="btn btn-outline-secondary ms-2" onClick={() => navigate(`/songs/${id}`)}>Отмена</button>
                </form>
            </div>
        </div>
    );
}