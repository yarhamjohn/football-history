import React from "react";
import ReactDOM from "react-dom";
import "./index.css";
import App from "./App";
import * as serviceWorker from "./serviceWorker";
import { AppContainer } from "react-hot-loader";
import { createStore } from "redux";
import { Provider } from "react-redux";

type ProjectState = {
  type: "LOADING_PROJECT";
};

type ProjectAction = {
  type: "PROJECT_LOAD_STARTED";
};

interface ApplicationState {
  projectState: ProjectState;
}
type ApplicationAction = ProjectAction;

const projectReducer = (
  prevState: ProjectState | undefined,
  action: ApplicationAction
): ProjectState => {
  if (!prevState) {
    return { type: "LOADING_PROJECT" };
  }

  return prevState;
};

const applicationReducer = (
  prevState: ApplicationState | undefined,
  action: ApplicationAction
): ApplicationState => {
  return {
    projectState: projectReducer(prevState?.projectState, action),
  };
};

const store = createStore<ApplicationState, ApplicationAction, unknown, unknown>(
  applicationReducer
);

ReactDOM.render(
  <AppContainer>
    <Provider store={store}>
      <App />
    </Provider>
  </AppContainer>,
  document.getElementById("root")
);

// If you want your app to work offline and load faster, you can change
// unregister() to register() below. Note this comes with some pitfalls.
// Learn more about service workers: https://bit.ly/CRA-PWA
serviceWorker.unregister();
