import { useEffect, useState } from "react";
import { api } from "../api/client";

export default function GenresAdminPage() {
    const [genres, setGenres] = useState([]);
    const [newName, setNewName] = useState("");
    const [editingId, setEditingId] = useState(null);
    const [editingName, setEditingName] = useState("");
    const [error, setError] = useState("");

    const load = () => api.getGenres().then(setGenres).catch((e) => setError(e.message));

    useEffect(() => { load(); }, []);

    const handleCreate = async (e) => {
        e.preventDefault();
        if (!newName.trim()) return;
        try {
            await api.createGenre(newName.trim());
            setNewName("");
            load();
        } catch (err) {
            setError(err.message);
        }
    };

    const startEdit = (g) => {
        setEditingId(g.id);
        setEditingName(g.name);
    };

    const saveEdit = async (id) => {
        try {
            await api.updateGenre(id, editingName.trim());
            setEditingId(null);
            load();
        } catch (err) {
            setError(err.message);
        }
    };

    const handleDelete = async (id) => {
        if (!window.confirm("Удалить жанр? Он будет отвязан от всех песен.")) return;
        try {
            await api.deleteGenre(id);
            load();
        } catch (err) {
            setError(err.message);
        }
    };

    return (
        <div>
            <h2 className="mb-4">Жанры</h2>
            {error && <div className="alert alert-danger">{error}</div>}

            <form onSubmit={handleCreate} className="d-flex gap-2 mb-4" style={{ maxWidth: 400 }}>
                <input
                    className="form-control"
                    placeholder="Новый жанр"
                    value={newName}
                    maxLength={50}
                    onChange={(e) => setNewName(e.target.value)}
                />
                <button type="submit" className="btn btn-primary text-nowrap">Добавить жанр</button>
            </form>

            <table className="table table-striped bg-white">
                <thead>
                    <tr><th>Название</th><th className="text-end">Действия</th></tr>
                </thead>
                <tbody>
                    {genres.map((g) => (
                        <tr key={g.id}>
                            <td>
                                {editingId === g.id ? (
                                    <input
                                        className="form-control form-control-sm"
                                        value={editingName}
                                        maxLength={50}
                                        onChange={(e) => setEditingName(e.target.value)}
                                    />
                                ) : (
                                    g.name
                                )}
                            </td>
                            <td className="text-end">
                                {editingId === g.id ? (
                                    <>
                                        <button className="btn btn-sm btn-success me-1" onClick={() => saveEdit(g.id)}>Сохранить</button>
                                        <button className="btn btn-sm btn-outline-secondary" onClick={() => setEditingId(null)}>Отмена</button>
                                    </>
                                ) : (
                                    <>
                                        <button className="btn btn-sm btn-outline-secondary me-1" onClick={() => startEdit(g)}>Изменить</button>
                                        <button className="btn btn-sm btn-outline-danger" onClick={() => handleDelete(g.id)}>Удалить</button>
                                    </>
                                )}
                            </td>
                        </tr>
                    ))}
                </tbody>
            </table>
        </div>
    );
}