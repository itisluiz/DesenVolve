import {
  Icon,
  InputAdornment,
  Stack,
  TextField,
  Typography,
} from "@mui/material";
import { LoadingButton } from "@mui/lab";
import { useState } from "react";

const FormCadastro = (props) => {
  const [nome, setNome] = useState("");
  const [sobrenome, setSobrenome] = useState("");
  const [email, setEmail] = useState("");
  const [senha, setSenha] = useState("");
  const [senhaRepetida, setSenhaRepetida] = useState("");

  function chamarCadastro() {
    props.submit(nome, sobrenome, email, senha);
  }

  return (
    <Stack
      display="flex"
      alignItems="center"
      justifyContent="center"
      minHeight="35em"
      gap="1em"
      p="0 10em"
    >
      <Typography variant="h5" color="primary">
        Ingressar no DesenVolve
      </Typography>
      <Typography variant="caption">Cria uma conta DesenVolve</Typography>

      <TextField
        fullWidth
        label="Nome"
        placeholder="Nome"
        InputProps={{
          startAdornment: (
            <InputAdornment position="start">
              <Icon>person</Icon>
            </InputAdornment>
          ),
        }}
        error={nome.length > 0 && nome.length < 2}
        value={nome}
        onChange={(e) => setNome(e.target.value)}
      />

      <TextField
        fullWidth
        label="Sobrenome"
        placeholder="Sobrenome"
        InputProps={{
          startAdornment: (
            <InputAdornment position="start">
              <Icon>person</Icon>
            </InputAdornment>
          ),
        }}
        error={sobrenome.length > 0 && sobrenome.length < 2}
        value={sobrenome}
        onChange={(e) => setSobrenome(e.target.value)}
      />

      <TextField
        fullWidth
        label="E-mail"
        placeholder="E-mail"
        InputProps={{
          startAdornment: (
            <InputAdornment position="start">
              <Icon>email</Icon>
            </InputAdornment>
          ),
        }}
        error={email.length > 0 && email.length < 2}
        value={email}
        onChange={(e) => setEmail(e.target.value)}
      />

      <TextField
        fullWidth
        label="Senha"
        placeholder="Senha"
        type="password"
        InputProps={{
          startAdornment: (
            <InputAdornment position="start">
              <Icon>password</Icon>
            </InputAdornment>
          ),
        }}
        error={senha.length > 0 && senha.length < 6}
        value={senha}
        onChange={(e) => setSenha(e.target.value)}
      />

      <TextField
        fullWidth
        label="Repita a Senha"
        placeholder="Confirme a senha"
        type="password"
        InputProps={{
          startAdornment: (
            <InputAdornment position="start">
              <Icon>password</Icon>
            </InputAdornment>
          ),
        }}
        error={senhaRepetida.length > 0 && senhaRepetida != senha}
        value={senhaRepetida}
        onChange={(e) => setSenhaRepetida(e.target.value)}
      />

      <LoadingButton
        loading={props.loading}
        fullWidth
        variant="outlined"
        disabled={
          nome.length <= 2 ||
          sobrenome.length < 2 ||
          email.length < 2 ||
          senha.length < 6 ||
          senha != senhaRepetida
        }
        onClick={chamarCadastro}
      >
        Cadastrar
      </LoadingButton>
    </Stack>
  );
};

export default FormCadastro;
