var SearchResult = React.createClass({
    render: function() {
        return (
            <table className="table table-striped">
                <thead>
                    <tr>
                        <th>#</th>
                        <th>First Name</th>
                        <th>Last Name</th>
                    </tr>
                </thead>
                <tbody>
                    {this.props.hits.map(function(){
                        <tr>
                            <td>{index}</td>
                            <td>{text}</td>
                            <td>{url}</td>
                        </tr>
                    })}
                </tbody>
            </table>
        );
    }
});

var hits = [];

ReactDOM.render(
  <SearchResult hits={hits} />,
  document.getElementById('content')
);


$(document).ready(function () {
    $('#btnSearch').click(function () {
        $.ajax({
            url: '/api/search',
            data: { keyword: $('#txtKeyword').val(), urlPrefix: $('#txtUrlPrefix').val() }
        }).done(function (res) {
            hits = res;
        });
    });
});