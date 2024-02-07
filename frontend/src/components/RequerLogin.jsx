import { useContext } from "react";
import { ContexoUsuario } from "../contexts/ContextoUsuario";
import { Navigate, useLocation } from "react-router-dom";

const RequerLogin = (props) => {
	const {usuario} = useContext(ContexoUsuario);
	const location = useLocation();

	if (!usuario)
		return <Navigate to="/autenticar" state={ { from: location.pathname } } />

	return (<>{props.children}</>);
};

export default RequerLogin;
