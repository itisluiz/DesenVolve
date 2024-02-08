import { ProviderUsuario } from "../contexts/ContextoUsuario";

const Contexo = (props) => {
  return <ProviderUsuario>{props.children}</ProviderUsuario>;
};

export default Contexo;
