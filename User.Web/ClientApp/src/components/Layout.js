import React, { Component } from 'react';
import { Container } from 'reactstrap';
import LayoutContext from './LayoutContext';
import { ToastContainer, toast } from 'react-toastify';
import 'react-toastify/dist/ReactToastify.css';


export class Layout extends Component {
  static displayName = Layout.name;


    notify = (text) => toast(text);

  render () {
    return (
      <div>
            <Container>
                <ToastContainer/>
                <LayoutContext.Provider value={{ notify: this.notify }}>
                        {this.props.children}
                </LayoutContext.Provider>
               
        </Container>
      </div>
    );
  }
}
