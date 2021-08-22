import React, { useEffect, useState } from 'react';

export function Pagination({ value, onChangePaginationapi }) {

	const [hasPrevious, setHasPrevious] = useState(false);
	const [hasNext, setHasNext] = useState(false);
	const [previousPageLink, setPreviousPageLink] = useState('');
	const [nextPageLink, setNextPageLink] = useState('');
	const [firstPageLink, setFirstPageLinkk] = useState(value.firstPageLink);
	const [lastPageLink, setLastPageLink] = useState(value.lastPageLink);

	const [currentPage, setCurrentPage] = useState(value.currentPage);
	const [pageSize, setPageSize] = useState(value.pageSize);

	useEffect(() => {
		gotoPage(createLink(currentPage))

	}, [pageSize]);

	useEffect(() => {
		console.log(currentPage);
		if (currentPage && currentPage > 0) {
			setHasPrevious(currentPage > 1);
			setHasNext(currentPage < value.totalPages);
		}
	}, [currentPage]);

	useEffect(() => {
		console.log(currentPage);
		if (currentPage && currentPage > 0) {
			if (currentPage > 1)
				setPreviousPageLink(createLink(currentPage - 1));
			if (currentPage < value.totalPages)
				setNextPageLink(createLink(currentPage + 1));
			setFirstPageLinkk(createLink(1));
			setLastPageLink(createLink(value.totalPages));
		}
	}, [currentPage, pageSize]);


	const createLink = (currentPage) => {
		const params = (new URL(value.firstPageLink));
		var queryParams = new URLSearchParams(params.search);
		queryParams.set("page", currentPage);
		queryParams.set("pageSize", pageSize);
		console.log(queryParams.toString());
		let link = `${params.origin}${params.pathname}?${queryParams.toString()}`;
		return link;
	}

	const gotoPage = (link) => {
		console.log( 'gotoPage' + link);
		const params = (new URL(link));
		console.log(params);
		var queryParams = new URLSearchParams(params.search);
		let c = queryParams.get("page")
		setCurrentPage(Number(c));
		console.log('link ' + link);
		onChangePaginationapi(link)
	}

	return (
		<div className="pagination">
			<button onClick={() => gotoPage(firstPageLink)} disabled={!hasPrevious}>
				{"<<"}
			</button>{" "}
			<button onClick={() => gotoPage(previousPageLink)}
			    disabled={!hasPrevious}>
			    {"<"}
			</button>{" "}
			{value.HasNext}
			<button onClick={() => gotoPage(nextPageLink)}
			    disabled={!hasNext}>
			    {">"}
			</button>{" "}
			<button onClick={() => gotoPage(lastPageLink)} disabled={!hasNext}>
			    {">>"}
			</button>{" "}
			<span>
			    Page{" "}
			    <strong>
					{currentPage} of {value.totalPages}
			    </strong>{" "}
			</span>
			<span>
			    | Go to page:{" "}
			    <input
			        type="number"
					defaultValue={currentPage}
			        onChange={(e) => {
			            const page = e.target.value ? Number(e.target.value) : 1;
						gotoPage(page > 0 && page <= value.totalPages ? createLink(page) : createLink(1))
			        }}
			        style={{ width: "100px" }}
			    />
			</span>{" "}
			<select
			    value={pageSize}
				onChange={(e) => {
					let pageSize = Number(e.target.value);
					setPageSize(pageSize);
			    }}
			>
			    {[5, 10, 20, 30, 40, 50].map((pageSize) => (
			        <option key={pageSize} value={pageSize}>
						{pageSize}
			        </option>
			    ))}
			</select>
		</div>
	);
}