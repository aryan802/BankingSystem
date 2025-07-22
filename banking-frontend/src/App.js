import React from 'react';
import { BrowserRouter, Routes, Route, Navigate } from 'react-router-dom';
import { AuthProvider, useAuth } from './auth/AuthContext';
import Navbar from './components/Navbar';
import Login from './pages/Login';
import Register from './pages/Register';       // ensure this exists
import Dashboard from './pages/Dashboard';
import Accounts from './pages/Accounts';
import Transfer from './pages/Transfer';
import Loans from './pages/Loans';
// import Transactions from './pages/Transactions'; // optional

// A wrapper that redirects to /login if not authenticated
function Private({ children }) {
    const { user } = useAuth();
    return user ? children : <Navigate to="/login" />;
}

function App() {
    return (
        <AuthProvider>
            <BrowserRouter>
                <Navbar />
                <Routes>
                    <Route path="/login" element={<Login />} />
                    <Route path="/register" element={<Register />} />

                    <Route path="/" element={<Navigate to="/dashboard" />} />
                    <Route path="/dashboard" element={<Private><Dashboard /></Private>} />
                    <Route path="/accounts" element={<Private><Accounts /></Private>} />
                    <Route path="/transfer" element={<Private><Transfer /></Private>} />
                    <Route path="/loans" element={<Private><Loans /></Private>} />
                    {/* <Route path="/transactions/:id" element={<Private><Transactions/></Private>} /> */}
                </Routes>
            </BrowserRouter>
        </AuthProvider>
    );
}

export default App;


