var SearchPanel = React.createClass({
    
    handleSearch(e) {
        e.preventDefault();

        $.ajax({
            url: '/api/search',
            data: {
                keyword: this.refs.keyword.value,
                urlPrefix: this.refs.urlPrefix.value
            }
        }).done(function (res) {
            this.props.search(res);
        }.bind(this));
        
    },

    render: function () {
        
        return (
            <div>
                <div className="page-header">
                    <h2>Google Search Result</h2>
                </div>

                <div className="panel panel-default">
                    <div className="panel-heading">Search Filters</div>
                    <div className="panel-body">
                        <form onSubmit={this.handleSearch}>

                            <fieldset>

                                <div className="form-group">
                                    <label htmlFor="keyword">Keyword:</label>
                                    <input name="keyword" type="text" className="form-control" ref="keyword" placeholder="Keyword" defaultValue="Online Title Search" />
                                </div>

                                <div className="form-group">
                                    <label htmlFor="urlPrefix">Url Prefix:</label>
                                    <input name="urlPrefix" type="text" className="form-control" ref="urlPrefix" placeholder="Url Prefix" defaultValue="www.infotrack.com" />
                                </div>

                                <button type="submit" className="btn btn-primary">Search</button>

                            </fieldset>

                        </form>
                    </div>
                </div>

            </div>
        );
    }

});

