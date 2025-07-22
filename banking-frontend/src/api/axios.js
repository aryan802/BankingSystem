// src/api/axios.js
import axios from 'axios';

const api = axios.create({
    baseURL: 'https://localhost:7224/api', // your backend URL
    headers: {
        'Content-Type': 'application/json',
    },
});

// 🔐 Add this interceptor to set the token on every request
api.interceptors.request.use(
    config => {
        const token = localStorage.getItem('token');
        if (token) {
            config.headers.Authorization = `Bearer ${token}`;
        }
        return config;
    },
    error => Promise.reject(error)
);

export default api;
