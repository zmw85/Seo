var SearchResult = React.createClass({
   
    render() {
        
        return (
            <div className="panel panel-default">
                <div className="panel-heading">
                    Search Results
                </div>
                <div className="panel-body">
                    <div className="table-responsive">
                        <table className="table table-striped table-bordered table-hover">
                            <thead>
                                <tr>
                                    <th>#</th>
                                    <th>Title</th>
                                    <th>URL</th>
                                </tr>
                            </thead>
                            <tbody>
                                {
                                    this.props.hits.map(function (hit) {
                                        return (
                                            <tr key={hit.index}>
                                                <td>{hit.index}</td>
                                                <td>{hit.text}</td>
                                                <td>{hit.url}</td>
                                            </tr>
                                        )
                                    })
                                }
                            </tbody>
                        </table>
                    </div>
                </div>
            </div>
        );
    }
});