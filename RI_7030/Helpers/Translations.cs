namespace RI_7030.Helpers
{
    /// <summary>
    /// Centralized translation dictionary for English ↔ Gujarati.
    /// Usage: T["key"] returns the translated string for the current language.
    /// </summary>
    public static class Translations
    {
        private static readonly Dictionary<string, Dictionary<string, string>> _dict = new()
        {
            // ── Navigation & Layout ──────────────────────────────────────────
            ["nav_home"]          = new() { ["en"] = "Home",          ["gu"] = "હોમ" },
            ["nav_orders"]        = new() { ["en"] = "Orders",        ["gu"] = "ઓર્ડર" },
            ["nav_inventory"]     = new() { ["en"] = "Inventory",     ["gu"] = "ઈન્વેન્ટરી" },
            ["nav_employees"]     = new() { ["en"] = "Employees",     ["gu"] = "કર્મચારીઓ" },
            ["nav_transactions"]  = new() { ["en"] = "Transactions",  ["gu"] = "વ્યવહારો" },
            ["nav_salary"]        = new() { ["en"] = "Salary",        ["gu"] = "પગાર" },
            ["nav_profile"]       = new() { ["en"] = "Profile",       ["gu"] = "પ્રોફાઈલ" },
            ["nav_logout"]        = new() { ["en"] = "Logout",        ["gu"] = "લૉગઆઉટ" },
            ["welcome"]           = new() { ["en"] = "Welcome",       ["gu"] = "સ્વાગત છે" },

            // ── Dashboard ────────────────────────────────────────────────────
            ["dashboard"]         = new() { ["en"] = "Dashboard",     ["gu"] = "ડેશબોર્ડ" },
            ["to_receive"]        = new() { ["en"] = "To Receive (Receivable)", ["gu"] = "મેળવવાના (લેણાં)" },
            ["to_pay"]            = new() { ["en"] = "To Pay (Payable)",        ["gu"] = "ચૂકવવાના (દેવાં)" },
            ["pending_orders"]    = new() { ["en"] = "Pending Orders",          ["gu"] = "બાકી ઓર્ડર" },
            ["net_balance"]       = new() { ["en"] = "Net Balance",             ["gu"] = "ચોખ્ખી બાકી" },
            ["pending_sell"]      = new() { ["en"] = "Pending Sell transactions →", ["gu"] = "બાકી વેચાણ વ્યવહારો →" },
            ["pending_buy"]       = new() { ["en"] = "Pending Buy transactions →",  ["gu"] = "બાકી ખરીદી વ્યવહારો →" },
            ["not_delivered"]     = new() { ["en"] = "Not yet Delivered →",          ["gu"] = "હજુ ડિલિવર નથી →" },
            ["recv_minus_pay"]    = new() { ["en"] = "Receivable – Payable →",       ["gu"] = "લેણાં – દેવાં →" },
            ["payments_receive"]  = new() { ["en"] = "Payments to Receive",    ["gu"] = "મેળવવાની ચુકવણી" },
            ["payments_make"]     = new() { ["en"] = "Payments to Make",       ["gu"] = "કરવાની ચુકવણી" },
            ["pending_orders_prod"]= new() { ["en"] = "Pending Orders – Production Status", ["gu"] = "બાકી ઓર્ડર – ઉત્પાદન સ્થિતિ" },
            ["view_all"]          = new() { ["en"] = "View All →",             ["gu"] = "બધું જુઓ →" },
            ["no_pending_recv"]   = new() { ["en"] = "No pending receivables 🎉", ["gu"] = "કોઈ બાકી લેણાં નથી 🎉" },
            ["no_pending_pay"]    = new() { ["en"] = "No pending payables 🎉",    ["gu"] = "કોઈ બાકી દેવાં નથી 🎉" },
            ["no_pending_orders"] = new() { ["en"] = "No pending orders! All caught up 🎉", ["gu"] = "કોઈ બાકી ઓર્ડર નથી! બધું પૂર્ણ 🎉" },
            ["today_sales"]       = new() { ["en"] = "Today's Sales",          ["gu"] = "આજનું વેચાણ" },
            ["low_stock"]         = new() { ["en"] = "Low Stock Items",        ["gu"] = "ઓછો સ્ટોક" },
            ["my_department"]     = new() { ["en"] = "My Department",          ["gu"] = "મારો વિભાગ" },
            ["today_sell_tx"]     = new() { ["en"] = "Today's sell transactions",  ["gu"] = "આજના વેચાણ વ્યવહારો" },
            ["stock_below_15"]    = new() { ["en"] = "Products with stock < 15",   ["gu"] = "સ્ટોક < ૧૫ વાળી પ્રોડક્ટ" },
            ["recent_tx"]         = new() { ["en"] = "Recent Transactions",        ["gu"] = "તાજેતરના વ્યવહારો" },
            ["no_tx_yet"]         = new() { ["en"] = "No transactions yet.",       ["gu"] = "હજુ કોઈ વ્યવહાર નથી." },

            // ── Common Table Headers ─────────────────────────────────────────
            ["order_id"]     = new() { ["en"] = "Order ID",    ["gu"] = "ઓર્ડર ID" },
            ["customer"]     = new() { ["en"] = "Customer",    ["gu"] = "ગ્રાહક" },
            ["product"]      = new() { ["en"] = "Product",     ["gu"] = "પ્રોડક્ટ" },
            ["qty"]          = new() { ["en"] = "Qty",         ["gu"] = "જથ્થો" },
            ["amount"]       = new() { ["en"] = "Amount",      ["gu"] = "રકમ" },
            ["due_date"]     = new() { ["en"] = "Due Date",    ["gu"] = "નિયત તારીખ" },
            ["progress"]     = new() { ["en"] = "Progress",    ["gu"] = "પ્રગતિ" },
            ["status"]       = new() { ["en"] = "Status",      ["gu"] = "સ્થિતિ" },
            ["actions"]      = new() { ["en"] = "Actions",     ["gu"] = "ક્રિયાઓ" },
            ["date"]         = new() { ["en"] = "Date",        ["gu"] = "તારીખ" },
            ["type"]         = new() { ["en"] = "Type",        ["gu"] = "પ્રકાર" },
            ["party"]        = new() { ["en"] = "Party",       ["gu"] = "પાર્ટી" },
            ["id"]           = new() { ["en"] = "ID",          ["gu"] = "ID" },
            ["email"]        = new() { ["en"] = "Email",       ["gu"] = "ઈમેલ" },

            // ── Status Labels ────────────────────────────────────────────────
            ["s_pending"]      = new() { ["en"] = "Pending",        ["gu"] = "બાકી" },
            ["s_in_production"]= new() { ["en"] = "In Production",  ["gu"] = "ઉત્પાદનમાં" },
            ["s_ready"]        = new() { ["en"] = "Ready",          ["gu"] = "તૈયાર" },
            ["s_delivered"]    = new() { ["en"] = "Delivered",       ["gu"] = "ડિલિવર થયું" },
            ["s_paid"]         = new() { ["en"] = "Paid",           ["gu"] = "ચૂકવેલ" },
            ["s_received"]     = new() { ["en"] = "Received",       ["gu"] = "મળેલ" },
            ["s_cancelled"]    = new() { ["en"] = "Cancelled",      ["gu"] = "રદ" },
            ["s_all"]          = new() { ["en"] = "All",            ["gu"] = "બધા" },

            // ── Orders Page ──────────────────────────────────────────────────
            ["orders_title"]     = new() { ["en"] = "📋 Orders",          ["gu"] = "📋 ઓર્ડર" },
            ["search_orders"]    = new() { ["en"] = "🔍 Search by customer, product, order ID…", ["gu"] = "🔍 ગ્રાહક, પ્રોડક્ટ, ઓર્ડર ID થી શોધો…" },
            ["search"]           = new() { ["en"] = "Search",             ["gu"] = "શોધો" },
            ["new_order"]        = new() { ["en"] = "＋ New Order",        ["gu"] = "＋ નવો ઓર્ડર" },
            ["no_orders"]        = new() { ["en"] = "No orders found.",   ["gu"] = "કોઈ ઓર્ડર મળ્યા નથી." },
            ["view"]             = new() { ["en"] = "View",              ["gu"] = "જુઓ" },
            ["edit"]             = new() { ["en"] = "Edit",              ["gu"] = "ફેરફાર" },
            ["del"]              = new() { ["en"] = "Del",               ["gu"] = "કાઢો" },
            ["dispatch"]         = new() { ["en"] = "🚚 Dispatch",       ["gu"] = "🚚 ડિસ્પેચ" },
            ["dispatch_confirm"] = new() { ["en"] = "Dispatch this order? This will deduct stock and mark as Delivered.", ["gu"] = "આ ઓર્ડર ડિસ્પેચ કરશો? સ્ટોક કપાશે અને ડિલિવર થશે." },
            ["delete_confirm"]   = new() { ["en"] = "Delete this order?",       ["gu"] = "આ ઓર્ડર કાઢશો?" },
            ["customer_name"]    = new() { ["en"] = "Customer Name",            ["gu"] = "ગ્રાહકનું નામ" },
            ["unit_price"]       = new() { ["en"] = "Unit Price (₹)",           ["gu"] = "એકમ કિંમત (₹)" },
            ["quantity"]         = new() { ["en"] = "Quantity",                 ["gu"] = "જથ્થો" },
            ["total_amount"]     = new() { ["en"] = "Total Amount",             ["gu"] = "કુલ રકમ" },
            ["create_order"]     = new() { ["en"] = "Create Order",             ["gu"] = "ઓર્ડર બનાવો" },
            ["save_changes"]     = new() { ["en"] = "Save Changes",             ["gu"] = "ફેરફાર સાચવો" },
            ["cancel"]           = new() { ["en"] = "Cancel",                   ["gu"] = "રદ કરો" },
            ["close"]            = new() { ["en"] = "Close",                    ["gu"] = "બંધ કરો" },
            ["auto_id"]          = new() { ["en"] = "Order ID (Auto)",          ["gu"] = "ઓર્ડર ID (ઓટો)" },
            ["edit_order"]       = new() { ["en"] = "Edit Order",               ["gu"] = "ઓર્ડર ફેરફાર" },
            ["order_details"]    = new() { ["en"] = "Order Details",            ["gu"] = "ઓર્ડર વિગતો" },
            ["select_product"]   = new() { ["en"] = "— Select Product —",       ["gu"] = "— પ્રોડક્ట પસંદ કરો —" },
            ["production"]       = new() { ["en"] = "Production",               ["gu"] = "ઉત્પાદન" },

            // ── Inventory Page ───────────────────────────────────────────────
            ["inventory_title"]  = new() { ["en"] = "📦 Inventory",              ["gu"] = "📦 ઈન્વેન્ટરી" },
            ["add_product"]      = new() { ["en"] = "＋ Add Product",             ["gu"] = "＋ પ્રોડક્ટ ઉમેરો" },
            ["search_inventory"] = new() { ["en"] = "🔍 Search by product name or ID…", ["gu"] = "🔍 પ્રોડક્ટ નામ અથવા ID થી શોધો…" },
            ["product_id"]       = new() { ["en"] = "Product ID",               ["gu"] = "પ્રોડક્ટ ID" },
            ["product_name"]     = new() { ["en"] = "Product Name",             ["gu"] = "પ્રોડક્ટ નામ" },
            ["stock"]            = new() { ["en"] = "Stock",                    ["gu"] = "સ્ટોક" },
            ["unit_cost"]        = new() { ["en"] = "Unit Cost",                ["gu"] = "એકમ ખર્ચ" },
            ["prod_stages"]      = new() { ["en"] = "Production Stages",        ["gu"] = "ઉત્પાદન તબક્કા" },
            ["no_products"]      = new() { ["en"] = "No products found.",       ["gu"] = "કોઈ પ્રોડક્ટ મળી નથી." },
            ["add_first"]        = new() { ["en"] = "Add your first product!",  ["gu"] = "તમારી પ્રથમ પ્રોડક્ટ ઉમેરો!" },
            ["save_product"]     = new() { ["en"] = "Save Product",             ["gu"] = "પ્રોડક્ટ સાચવો" },
            ["update_product"]   = new() { ["en"] = "Update Product",           ["gu"] = "પ્રોડક્ટ અપડેટ કરો" },
            ["add_new_product"]  = new() { ["en"] = "+ Add New Product",        ["gu"] = "+ નવી પ્રોડક્ટ ઉમેરો" },
            ["edit_product"]     = new() { ["en"] = "Edit Product",             ["gu"] = "પ્રોડક્ટ ફેરફાર" },
            ["stock_qty"]        = new() { ["en"] = "Stock Quantity",           ["gu"] = "સ્ટોક જથ્થો" },
            ["stages_check"]     = new() { ["en"] = "Production Stages (check if complete)", ["gu"] = "ઉત્પાદન તબક્કા (પૂર્ણ હોય તો ટિક કરો)" },
            ["pcs"]              = new() { ["en"] = "pcs",                      ["gu"] = "નંગ" },
            ["done"]             = new() { ["en"] = "Done",                     ["gu"] = "પૂર્ણ" },

            // ── Stage Names ──────────────────────────────────────────────────
            ["casting"]          = new() { ["en"] = "Casting",             ["gu"] = "કાસ્ટિંગ" },
            ["finishing"]        = new() { ["en"] = "Finishing",            ["gu"] = "ફિનિશિંગ" },
            ["gold_plating"]     = new() { ["en"] = "Gold Plating",        ["gu"] = "ગોલ્ડ પ્લેટિંગ" },
            ["packaging"]        = new() { ["en"] = "Packaging",           ["gu"] = "પેકેજિંગ" },

            // ── Transactions Page ────────────────────────────────────────────
            ["tx_title"]           = new() { ["en"] = "Transactions",            ["gu"] = "વ્યવહારો" },
            ["buy"]                = new() { ["en"] = "💰 Buy",                  ["gu"] = "💰 ખરીદી" },
            ["sell"]               = new() { ["en"] = "💵 Sell",                 ["gu"] = "💵 વેચાણ" },
            ["history"]            = new() { ["en"] = "📋 History",              ["gu"] = "📋 ઈતિહાસ" },
            ["due_payments"]       = new() { ["en"] = "⏰ Due Payments",          ["gu"] = "⏰ બાકી ચુકવણી" },
            ["new_purchase"]       = new() { ["en"] = "New Purchase (Buy)",      ["gu"] = "નવી ખરીદી (Buy)" },
            ["new_sale"]           = new() { ["en"] = "New Sale (Sell)",          ["gu"] = "નવું વેચાણ (Sell)" },
            ["tx_history"]         = new() { ["en"] = "Transaction History",     ["gu"] = "વ્યવહાર ઈતિહાસ" },
            ["tx_id"]              = new() { ["en"] = "Tx ID",                   ["gu"] = "Tx ID" },
            ["tx_id_auto"]         = new() { ["en"] = "Transaction ID (Auto)",   ["gu"] = "વ્યવહાર ID (ઓટો)" },
            ["from_supplier"]      = new() { ["en"] = "From (Supplier Name)",    ["gu"] = "કોની પાસેથી (સપ્લાયર)" },
            ["to_customer"]        = new() { ["en"] = "To (Customer Name)",      ["gu"] = "કોને (ગ્રાહક)" },
            ["category"]           = new() { ["en"] = "Category",                ["gu"] = "શ્રેણી" },
            ["payment_method"]     = new() { ["en"] = "Payment Method",          ["gu"] = "ચુકવણી પદ્ધતિ" },
            ["complete_purchase"]   = new() { ["en"] = "Complete Purchase",      ["gu"] = "ખરીદી પૂર્ણ કરો" },
            ["complete_sale"]       = new() { ["en"] = "Complete Sale",          ["gu"] = "વેચાણ પૂર્ણ કરો" },
            ["search_tx"]           = new() { ["en"] = "🔍 Search by ID, party or product…", ["gu"] = "🔍 ID, પાર્ટી અથવા પ્રોડક્ટ થી શોધો…" },
            ["no_tx"]               = new() { ["en"] = "No transactions yet.",   ["gu"] = "હજુ કોઈ વ્યવહાર નથી." },
            ["due_title"]           = new() { ["en"] = "Due Payments",           ["gu"] = "બાકી ચુકવણી" },
            ["no_due"]              = new() { ["en"] = "No pending payments! 🎉", ["gu"] = "કોઈ બાકી ચુકવણી નથી! 🎉" },
            ["pay_now"]             = new() { ["en"] = "Pay Now",                ["gu"] = "હવે ચૂકવો" },
            ["mark_received"]       = new() { ["en"] = "Mark Received",          ["gu"] = "મળ્યું ચિહ્નિત કરો" },
            ["edit_tx"]             = new() { ["en"] = "Edit Transaction",       ["gu"] = "વ્યવહાર ફેરફાર" },
            ["party_name"]          = new() { ["en"] = "Party Name",             ["gu"] = "પાર્ટીનું નામ" },
            ["update"]              = new() { ["en"] = "Update",                 ["gu"] = "અપડેટ" },
            ["product_purchase"]    = new() { ["en"] = "Product Purchase",       ["gu"] = "પ્રોડક્ટ ખરીદી" },
            ["expense"]             = new() { ["en"] = "Expense",                ["gu"] = "ખર્ચ" },
            ["emp_salary"]          = new() { ["en"] = "Employee Salary",        ["gu"] = "કર્મચારી પગાર" },
            ["pending_not_paid"]    = new() { ["en"] = "— Pending (not paid) —", ["gu"] = "— બાકી (ચૂકવેલ નથી) —" },
            ["cash_paid"]           = new() { ["en"] = "Cash (Paid)",            ["gu"] = "રોકડ (ચૂકવેલ)" },
            ["bank_paid"]           = new() { ["en"] = "Bank Transfer (Paid)",   ["gu"] = "બેંક ટ્રાન્સફર (ચૂકવેલ)" },
            ["upi_paid"]            = new() { ["en"] = "UPI (Paid)",             ["gu"] = "UPI (ચૂકવેલ)" },
            ["pending_not_recv"]    = new() { ["en"] = "— Pending (not received) —", ["gu"] = "— બાકી (મળ્યું નથી) —" },
            ["cash_recv"]           = new() { ["en"] = "Cash (Received)",        ["gu"] = "રોકડ (મળેલ)" },
            ["bank_recv"]           = new() { ["en"] = "Bank Transfer (Received)", ["gu"] = "બેંક ટ્રાન્સફર (મળેલ)" },
            ["upi_recv"]            = new() { ["en"] = "UPI (Received)",         ["gu"] = "UPI (મળેલ)" },
            ["select_product_opt"]  = new() { ["en"] = "— Select Product (optional) —", ["gu"] = "— પ્રોડક્ટ પસંદ કરો (વૈકલ્પિક) —" },
            ["not_specified"]       = new() { ["en"] = "— Not specified —",      ["gu"] = "— નક્કી નથી —" },
            ["cash"]                = new() { ["en"] = "Cash",                   ["gu"] = "રોકડ" },
            ["bank_transfer"]       = new() { ["en"] = "Bank Transfer",          ["gu"] = "બેંક ટ્રાન્સફર" },

            // ── Employees Page ───────────────────────────────────────────────
            ["employees_title"]  = new() { ["en"] = "👥 Employees",              ["gu"] = "👥 કર્મચારીઓ" },
            ["new_hire"]         = new() { ["en"] = "＋ New Hire",                ["gu"] = "＋ નવી ભરતી" },
            ["search_emp"]       = new() { ["en"] = "Search by name, email or department…", ["gu"] = "નામ, ઈમેલ અથવા વિભાગ થી શોધો…" },
            ["employee"]         = new() { ["en"] = "Employee",                  ["gu"] = "કર્મચારી" },
            ["department"]       = new() { ["en"] = "Department",                ["gu"] = "વિભાગ" },
            ["salary"]           = new() { ["en"] = "Salary",                    ["gu"] = "પગાર" },
            ["joined"]           = new() { ["en"] = "Joined",                    ["gu"] = "જોડાયા" },
            ["no_employees"]     = new() { ["en"] = "No employees found.",       ["gu"] = "કોઈ કર્મચારી મળ્યા નથી." },
            ["full_name"]        = new() { ["en"] = "Full Name",                 ["gu"] = "પૂરું નામ" },
            ["email_address"]    = new() { ["en"] = "Email Address",             ["gu"] = "ઈમેલ" },
            ["create_record"]    = new() { ["en"] = "Create Record",             ["gu"] = "રેકોર્ડ બનાવો" },
            ["update_employee"]  = new() { ["en"] = "Update Employee",           ["gu"] = "કર્મચારી અપડેટ કરો" },
            ["pay_salary"]       = new() { ["en"] = "Pay Salary",                ["gu"] = "પગાર ચૂકવો" },
            ["payment_mode"]     = new() { ["en"] = "Payment Mode",              ["gu"] = "ચુકવણી રીત" },
            ["confirm_payment"]  = new() { ["en"] = "Confirm Payment",           ["gu"] = "ચુકવણી કન્ફર્મ કરો" },
            ["pay"]              = new() { ["en"] = "Pay",                       ["gu"] = "ચૂકવો" },
            ["emp_id_auto"]      = new() { ["en"] = "Employee ID (Auto)",        ["gu"] = "કર્મચારી ID (ઓટો)" },
            ["finishing_touch"]  = new() { ["en"] = "Finishing Touch",            ["gu"] = "ફિનિશિંગ ટચ" },
            ["delete_confirm_emp"] = new() { ["en"] = "Delete this employee?",   ["gu"] = "આ કર્મચારીને કાઢશો?" },

            // ── Salary Page ──────────────────────────────────────────────────
            ["salary_pending_title"] = new() { ["en"] = "💰 Pending Advance Salary Requests", ["gu"] = "💰 બાકી એડવાન્સ પગાર વિનંતીઓ" },
            ["my_salary"]            = new() { ["en"] = "💼 My Salary",               ["gu"] = "💼 મારો પગાર" },
            ["request_advance"]      = new() { ["en"] = "+ Request Advance",          ["gu"] = "+ એડવાન્સ વિનંતી" },
            ["no_pending_adv"]       = new() { ["en"] = "No pending advance requests 🎉", ["gu"] = "કોઈ બાકી એડવાન્સ વિનંતી નથી 🎉" },
            ["no_salary_records"]    = new() { ["en"] = "No salary records yet.",      ["gu"] = "હજુ કોઈ પગાર રેકોર્ડ નથી." },
            ["requested"]            = new() { ["en"] = "Requested",                  ["gu"] = "વિનંતી" },
            ["advance"]              = new() { ["en"] = "Advance",                    ["gu"] = "એડવાન્સ" },
            ["salary_label"]         = new() { ["en"] = "Salary",                     ["gu"] = "પગાર" },
            ["request_salary_adv"]   = new() { ["en"] = "Request Salary Advance",     ["gu"] = "પગાર એડવાન્સ વિનંતી" },
            ["advance_amount"]       = new() { ["en"] = "Advance Amount (₹)",         ["gu"] = "એડવાન્સ રકમ (₹)" },
            ["reason_optional"]      = new() { ["en"] = "Reason (optional)",          ["gu"] = "કારણ (વૈકલ્પિક)" },
            ["submit_request"]       = new() { ["en"] = "Submit Request",             ["gu"] = "વિનંતી મોકલો" },
            ["pay_adv_confirm"]      = new() { ["en"] = "Pay this advance?",          ["gu"] = "આ એડવાન્સ ચૂકવશો?" },

            // ── Profile Page ─────────────────────────────────────────────────
            ["personal_info"]    = new() { ["en"] = "Personal Information",     ["gu"] = "અંગત માહિતી" },
            ["edit_profile"]     = new() { ["en"] = "Edit Profile",             ["gu"] = "પ્રોફાઈલ ફેરફાર" },
            ["change_password"]  = new() { ["en"] = "Change Password",          ["gu"] = "પાસવર્ડ બદલો" },
            ["phone"]            = new() { ["en"] = "Phone",                    ["gu"] = "ફોન" },
            ["bio"]              = new() { ["en"] = "Bio",                      ["gu"] = "બાયો" },
            ["tax_id"]           = new() { ["en"] = "Tax ID",                   ["gu"] = "ટેક્સ ID" },
            ["country"]          = new() { ["en"] = "Country",                  ["gu"] = "દેશ" },
            ["city_state"]       = new() { ["en"] = "City / State",             ["gu"] = "શહેર / રાજ્ય" },
            ["postal_code"]      = new() { ["en"] = "Postal Code",              ["gu"] = "પિન કોડ" },
            ["address"]          = new() { ["en"] = "Address",                  ["gu"] = "સરનામું" },
            ["current_password"] = new() { ["en"] = "Current Password",         ["gu"] = "હાલનો પાસવર્ડ" },
            ["new_password"]     = new() { ["en"] = "New Password (min 6 chars)", ["gu"] = "નવો પાસવર્ડ (ઓછામાં ઓછા ૬ અક્ષર)" },
            ["confirm_password"] = new() { ["en"] = "Confirm New Password",     ["gu"] = "નવો પાસવર્ડ કન્ફર્મ કરો" },
            ["language"]         = new() { ["en"] = "Language",                 ["gu"] = "ભાષા" },
            ["month"]            = new() { ["en"] = "/month",                   ["gu"] = "/મહિનો" },
            ["tax_pan"]          = new() { ["en"] = "Tax ID / PAN",             ["gu"] = "ટેક્સ ID / PAN" },
        };

        /// <summary>
        /// Get a dictionary of key→translated_string for the given language code ("en" or "gu").
        /// </summary>
        public static Dictionary<string, string> For(string lang)
        {
            var result = new Dictionary<string, string>();
            foreach (var kv in _dict)
            {
                result[kv.Key] = kv.Value.TryGetValue(lang, out var val) ? val : kv.Value["en"];
            }
            return result;
        }
    }
}
