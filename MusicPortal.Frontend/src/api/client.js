const API_BASE_URL = import.meta.env.VITE_API_BASE_URL || "https://localhost:7100/api";

async function request(path, options = {}) {
    const res = await fetch(`${API_BASE_URL}${path}`, {
        credentials: "include",
        headers: { "Content-Type": "application/json", ...(options.headers || {}) },
        ...options,
    });

    if (!res.ok) {
        const message = await res.text().catch(() => res.statusText);
        throw new Error(message || `Request failed: ${res.status}`);
    }

    if (res.status === 204) return null;
    return res.json();
}

async function requestForm(path, formData, method = "POST") {
    const res = await fetch(`${API_BASE_URL}${path}`, {
        method,
        credentials: "include",
        body: formData,
    });
    if (!res.ok) {
        const message = await res.text().catch(() => res.statusText);
        throw new Error(message || `Request failed: ${res.status}`);
    }
    if (res.status === 204) return null;
    return res.json();
}

export const api = {
    getSongs: (params = {}) => request(`/songs?${new URLSearchParams(params)}`),
    getSong: (id) => request(`/songs/${id}`),
    getComments: (id, page = 1) => request(`/songs/${id}/comments?page=${page}`),
    addComment: (id, text) => request(`/songs/${id}/comments`, { method: "POST", body: JSON.stringify({ text }) }),
    rateSong: (id, value) => request(`/songs/${id}/rate`, { method: "POST", body: JSON.stringify({ value }) }),
    uploadSong: (formData) => requestForm(`/songs`, formData),
    updateSong: (id, payload) => request(`/songs/${id}`, { method: "PUT", body: JSON.stringify(payload) }),
    deleteSong: (id) => request(`/songs/${id}`, { method: "DELETE" }),
    playUrl: (id) => `${API_BASE_URL}/songs/${id}/play`,
    downloadUrl: (id) => `${API_BASE_URL}/songs/${id}/download`,

    getGenres: () => request(`/genres`),
    createGenre: (name) => request(`/genres`, { method: "POST", body: JSON.stringify({ name }) }),
    updateGenre: (id, name) => request(`/genres/${id}`, { method: "PUT", body: JSON.stringify({ id, name }) }),
    deleteGenre: (id) => request(`/genres/${id}`, { method: "DELETE" }),

    getArtists: () => request(`/artists`),
    getArtistsBrief: () => request(`/artists/brief`),
    getArtist: (id) => request(`/artists/${id}`),
    createArtist: (name, bio) => request(`/artists`, { method: "POST", body: JSON.stringify({ name, bio }) }),

    login: (email, password, rememberMe) =>
        request(`/account/login`, { method: "POST", body: JSON.stringify({ email, password, rememberMe }) }),
    register: (payload) => request(`/account/register`, { method: "POST", body: JSON.stringify(payload) }),
    logout: () => request(`/account/logout`, { method: "POST" }),
    me: () => request(`/account/me`),

    getUsers: (page = 1) => request(`/admin/users?page=${page}`),
    getUser: (id) => request(`/admin/users/${id}`),
    getPendingUsers: () => request(`/admin/users/pending`),
    getRoles: () => request(`/admin/roles`),
    updateUser: (id, roleIds, isApproved) =>
        request(`/admin/users/${id}`, { method: "PUT", body: JSON.stringify({ roleIds, isApproved }) }),
    approveUser: (id) => request(`/admin/users/${id}/approve`, { method: "POST" }),
    rejectUser: (id) => request(`/admin/users/${id}/reject`, { method: "POST" }),
    deleteUser: (id) => request(`/admin/users/${id}`, { method: "DELETE" }),
};