import { useState } from "react";
import { useNavigate } from "react-router-dom";
import { api } from "../api/client";

export default function ArtistCreatePage() {
    const navigate = useNavigate();
    const [name, setName] = useState("");
    const [bio, setBio] = useState("");
    const [error, setError] = useState("");

    const handleSubmit = async (e) => {
        e.preventDefault();
        setError("");
        try {
            const artistId = await api.createArtist(name.trim(), bio.trim());
            navigate(`/artists/${artistId}`);
        } catch (err) {
            setError(err.message);
        }
    };

    return (
        <div className="row justify-content-center">
            <div className="col-md-5">
                <h2 className="mb-4">Новый исполнитель</h2>
                {error && <div className="alert alert-danger">{error}</div>}
                <form onSubmit={handleSubmit}>
                    <div className="mb-3">
                        <label className="form-label">Имя</label>
                        <input
                            className="form-control"
                            value={name}
                            onChange={(e) => setName(e.target.value)}
                            maxLength={100}
                            required
                        />
                    </div>
                    <div className="mb-3">
                        <label className="form-label">Биография</label>
                        <textarea
                            className="form-control"
                            rows={3}
                            value={bio}
                            onChange={(e) => setBio(e.target.value)}
                            maxLength={1000}
                        />
                    </div>
                    <button type="submit" className="btn btn-primary">Создать</button>
                    <button type="button" className="btn btn-outline-secondary ms-2" onClick={() => navigate(-1)}>Отмена</button>
                </form>
            </div>
        </div>
    );
}