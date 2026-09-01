import { useEffect, useState } from "react";
import { useParams, Link } from "react-router-dom";
import { api } from "../api/client";

export default function ArtistDetailsPage() {
    const { id } = useParams();
    const [artist, setArtist] = useState(null);
    const [error, setError] = useState("");

    useEffect(() => {
        api.getArtist(id).then(setArtist).catch((e) => setError(e.message));
    }, [id]);

    if (error) return <div className="alert alert-danger">{error}</div>;
    if (!artist) return <p>Загрузка...</p>;

    return (
        <div>
            <h2>{artist.name}</h2>
            {artist.bio && <p className="text-muted">{artist.bio}</p>}

            <h5 className="mt-4">Песни</h5>
            {artist.songs.length === 0 ? (
                <p className="text-muted">У исполнителя пока нет песен.</p>
            ) : (
                <table className="table bg-white">
                    <thead>
                        <tr><th>Название</th><th>Рейтинг</th><th>Прослушивания</th><th>Дата загрузки</th></tr>
                    </thead>
                    <tbody>
                        {[...artist.songs].sort((a, b) => new Date(b.uploadDate) - new Date(a.uploadDate)).map((song) => (
                            <tr key={song.id}>
                                <td><Link to={`/songs/${song.id}`}>{song.title}</Link></td>
                                <td>⭐ {song.rating.toFixed(1)}</td>
                                <td>{song.playCount}</td>
                                <td>{new Date(song.uploadDate).toLocaleDateString("ru-RU")}</td>
                            </tr>
                        ))}
                    </tbody>
                </table>
            )}
        </div>
    );
}