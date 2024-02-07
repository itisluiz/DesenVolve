import { ProviderUsuario } from "../contexts/ContextoUsuario";

const Contexto = (props) => {
	return (
		<ProviderUsuario>
			{props.children}
		</ProviderUsuario>
	);
};

export default Contexto;
 