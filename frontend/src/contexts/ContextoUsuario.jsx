import { createContext, useState } from 'react';

const ContexoUsuario = createContext();

const ProviderUsuario = (props) => {
	const [usuario, setUsuario] = useState({
		nome: 'Fulano'
	});

	return (
		<ContexoUsuario.Provider value={{ usuario, setUsuario }}>
			{props.children}
		</ContexoUsuario.Provider>
	);
};

export { ProviderUsuario, ContexoUsuario };
