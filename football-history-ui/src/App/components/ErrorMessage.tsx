import React, { FunctionComponent } from "react";
import { Message, MessageContent, MessageHeader } from "semantic-ui-react";

const ErrorMessage: FunctionComponent<{
  header: string;
  content: string;
}> = ({ header, content }) => {
  return (
    <Message negative>
      <MessageHeader>{header}</MessageHeader>
      <MessageContent>{content}</MessageContent>
    </Message>
  );
};

export { ErrorMessage };
