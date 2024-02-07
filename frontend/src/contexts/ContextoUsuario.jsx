import { createContext, useState } from 'react';

const ContextoUsuario = createContext();

const ProviderUsuario = (props) => {
	const [usuario, setUsuario] = useState(null);

	return (
		<ContextoUsuario.Provider value={{ usuario, setUsuario }}>
			{props.children}
		</ContextoUsuario.Provider>
	);
};

export { ProviderUsuario, ContextoUsuario };
