<script>
    function addToCart(maSp) {
        $.ajax({
            url: '/Cart/AddToCart', // Đường dẫn đến action xử lý việc thêm sản phẩm vào giỏ hàng
            type: 'POST',
            data: { maSp: maSp }, // Truyền mã sản phẩm đến action
            success: function (result) {
                // Xử lý kết quả trả về nếu cần thiết
                alert('Sản phẩm đã được thêm vào giỏ hàng!');
            },
            error: function (error) {
                // Xử lý lỗi nếu có
                console.error(error);
            }
        });
    }
</script>
