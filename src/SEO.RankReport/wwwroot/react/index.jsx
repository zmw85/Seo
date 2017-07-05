
var App = React.createClass({
    getInitialState() {
        return {
            hits: []
        };
    },

    handleSearch(hits) {
        this.setState({ hits: hits });
    },

    render: function () {
        return (
            <div>
                <SearchPanel search={this.handleSearch.bind(this)} />
                <SearchResult hits={this.state.hits} />
            </div>
        )
    }
});

ReactDOM.render(
  <App />,
  document.getElementById('content')
);
