public interface IItemVisitor
{
    void Visit(Item item, ItemVisitParams visitParams) => Visit((dynamic)item, visitParams);

    void Visit(Magnet magnet, ItemVisitParams visitParams);

    void Visit(Star star, ItemVisitParams visitParams);

    void Visit(Feather feather, ItemVisitParams visitParams);

    void Visit(Wrench wrench, ItemVisitParams visitParams);

    void Visit(Nut nut, ItemVisitParams visitParams);
}