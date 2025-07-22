import React, { createContext, useContext, useState, useEffect } from 'react';

const AuthContext = createContext();

export const AuthProvider = ({ children }) => {
    const [user, setUser] = useState(false);

    // On mount, check if token exists
    useEffect(() => {
        if (localStorage.getItem('token')) {
            setUser(true);
        }
    }, []);

    const login = (token) => {
        localStorage.setItem('token', token);
        setUser(true);
    };

    const logout = () => {
        localStorage.removeItem('token');
        setUser(false);
    };

    return (
        <AuthContext.Provider value={{ user, login, logout }}>
            {children}
        </AuthContext.Provider>
    );
};

export const useAuth = () => useContext(AuthContext);

