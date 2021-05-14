import { useEffect, useState } from "react";

export type FetchResult =
  | {
      status: "UNLOADED";
    }
  | { status: "LOADING" }
  | { status: "LOAD_SUCCESSFUL"; data: any }
  | { status: "LOAD_FAILED"; error: string };

const useFetch = (url: string) => {
  const [result, setResult] = useState<FetchResult>({ status: "UNLOADED" });

  useEffect(() => {
    if (url === "") {
      return;
    }

    const abortController = new AbortController();

    setResult({ status: "LOADING" });

    fetch(url, { signal: abortController.signal })
      .then((response) => response.json())
      .then((response) => {
        if (response.error === null) {
          setResult({ status: "LOAD_SUCCESSFUL", data: response.result });
        } else {
          throw new Error(response.error.message);
        }
      })
      .catch((error) => {
        if (!abortController.signal.aborted) {
          setResult({ status: "LOAD_FAILED", error: error });
        }
      });

    return () => {
      abortController.abort();
    };
  }, [url]);

  return result;
};

export { useFetch };
