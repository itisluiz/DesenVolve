import { ThemeProvider, createTheme } from "@mui/material/styles";
import { CssBaseline } from "@mui/material";

const temaPadrao = createTheme();

const Tema = (props) => {
  return (
    <ThemeProvider theme={temaPadrao}>
      <CssBaseline />
      {props.children}
    </ThemeProvider>
  );
};

export default Tema;
